using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Matrix.Api;
using Matrix.Api.Versions;
using Matrix.Backends;
using Matrix.Properties;
using Matrix.Structures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

[assembly:NeutralResourcesLanguage("en")]

/**
 * This class contains all the methods needed to call the Matrix C2S API. The methods are split into files
 * inside ./Api.
 */

namespace Matrix
{
    public delegate void MatrixApiRoomJoinedDelegate(string roomid, MatrixEventRoomJoined joined);

    public delegate void MatrixApiRoomInviteDelegate(string roomid, MatrixEventRoomInvited invited);

    // We need to mock MatrixApi, hence needing victuals.
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public partial class MatrixApi
    {
        public virtual Uri BaseUrl { get; private set; }
        public virtual string UserId { get; set; }
        public IMatrixApiBackend Backend { get; }
        public Random Rng { get; }
        public JsonEventConverter EventConverter { get; }

        public ILogger Log { get; } = Logger.Factory.CreateLogger<MatrixApi>();

        public RoomApi Room { get; }
        public LoginApi Login { get; }
        public DeviceApi Device { get; }
        public MediaApi Media { get; }
        public ProfileApi Profile { get; }
        public SyncApi Sync { get; }
        public RoomDirectoryApi RoomDirectory { get; }

        private MatrixVersions _versions;
        private readonly bool _isAppservice;
        private MatrixLoginResponse _currentLogin;

        private static JsonSerializer MatrixSerializer { get; } = new JsonSerializer();

        public MatrixApi(Uri url)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException(Resources.InvalidUrl);

            _isAppservice = false;
            Backend = new HttpBackend(url);
            BaseUrl = url;
            Rng = new Random(DateTime.Now.Millisecond);
            EventConverter = new JsonEventConverter();

            Room = new RoomApi(this);
            Login = new LoginApi(this);
            Device = new DeviceApi(this);
            Media = new MediaApi(this);
            Profile = new ProfileApi(this);
            Sync = new SyncApi(this);
            RoomDirectory = new RoomDirectoryApi(this);
        }

        public MatrixApi(Uri url, string applicationToken, string userId)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException(Resources.InvalidUrl);

            _isAppservice = true;
            Backend = new HttpBackend(url, userId);
            Backend.SetAccessToken(applicationToken);
            UserId = userId;
            BaseUrl = url;
            Rng = new Random(DateTime.Now.Millisecond);
            EventConverter = new JsonEventConverter();

            Room = new RoomApi(this);
            Login = new LoginApi(this);
            Device = new DeviceApi(this);
            Media = new MediaApi(this);
            Profile = new ProfileApi(this);
            Sync = new SyncApi(this);
            RoomDirectory = new RoomDirectoryApi(this);
        }

        public MatrixApi(Uri url, IMatrixApiBackend backend)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException(Resources.InvalidUrl);

            _isAppservice = true;
            Backend = backend;
            BaseUrl = url;
            Rng = new Random(DateTime.Now.Millisecond);
            EventConverter = new JsonEventConverter();

            Room = new RoomApi(this);
            Login = new LoginApi(this);
            Device = new DeviceApi(this);
            Media = new MediaApi(this);
            Profile = new ProfileApi(this);
            Sync = new SyncApi(this);
            RoomDirectory = new RoomDirectoryApi(this);
        }


        public void AddMessageType(string name, Type type)
        {
            EventConverter.AddMessageType(name, type);
        }

        public void AddEventType(string messageType, Type type)
        {
            EventConverter.AddEventType(messageType, type);
        }

        public virtual string GetAccessToken()
        {
            return _currentLogin?.AccessToken;
        }

        public virtual MatrixLoginResponse GetCurrentLogin()
        {
            return _currentLogin;
        }

        public void SetLogin(MatrixLoginResponse response)
        {
            _currentLogin = response ?? throw new ArgumentNullException(nameof(response));
            UserId = response.UserId;
            Backend.SetAccessToken(response.AccessToken);
        }

        public static JObject ObjectToJson(object data)
        {
            JObject container;

            using var writer = new JTokenWriter();
            try
            {
                MatrixSerializer.Serialize(writer, data);
                container = (JObject) writer.Token;
            }
            catch (Exception e)
            {
                throw new Exception(Resources.ObjToJsonFail, e);
            }

            return container;
        }

        public bool IsLoggedIn()
        {
            //TODO: Check token is still valid
            return _currentLogin != null;
        }

        [MatrixSpec(ClientServerApiVersion.R001, "get-matrix-client-versions")]
        public MatrixVersions ClientVersions()
        {
            var apiPath = new Uri("/_matrix/client/versions", UriKind.Relative);
            var error = Backend.HandleGet(apiPath, false, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            var res = result.ToObject<MatrixVersions>();
            _versions = res;
            return res;
        }

        [MatrixSpec(ClientServerApiVersion.R040, "get-matrix-client-r0-joined_rooms")]
        public List<string> GetJoinedRooms()
        {
            var apiPath = new Uri("/_matrix/client/r0/joined_rooms", UriKind.Relative);
            var error = Backend.HandleGet(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return (result as JObject)?.GetValue("joined_rooms", StringComparison.InvariantCulture)
                .ToObject<List<string>>();
        }

        [MatrixSpec(ClientServerApiVersion.R040, "get-matrix-client-r0-joined_members")]
        public Dictionary<string, MatrixProfile> GetJoinedMembers(string roomId)
        {
            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/joined_members", UriKind.Relative);
            var error = Backend.HandleGet(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return (result as JObject)?.GetValue("joined", StringComparison.InvariantCulture)
                .ToObject<Dictionary<string, MatrixProfile>>();
        }

        public void RegisterUserAsAppservice(string user)
        {
            if (!_isAppservice)
                throw new MatrixException(Resources.AppserviceNotRegistered);

            var request = JObject.FromObject(
                new
                {
                    type = "m.login.application_service",
                    user
                });

            var apiPath = new Uri("/_matrix/client/r0/register", UriKind.Relative);
            var error = Backend.HandlePost(apiPath, true, request, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        public void ThrowIfNotSupported([CallerMemberName] string name = null)
        {
            if (name == null) return;

            if (_versions == null)
                ClientVersions();

            if (!((typeof(MatrixApi).GetMethod(name) ?? throw new InvalidOperationException()).GetCustomAttribute(
                typeof(MatrixSpecAttribute)) is MatrixSpecAttribute spec))
            {
#if DEBUG
                Log.LogWarning($"{name} has no MatrixSpec attribute, cannot determine homeserver support");
#endif
                return;
            }

            var clientServerContext = spec.ApiVersionContext.Api switch
            {
                MatrixSpecApi.ClientServer => (spec.ApiVersionContext as ApiVersionContext.ClientServer)
            };

            if (clientServerContext == null) throw new EndOfStreamException(nameof(clientServerContext));

            // Ensure we support a version of the spec >= the min version and <= the last version.
            if (!_versions.SupportedVersions()
                .Any(version => version >= clientServerContext.MinVersion && version <= clientServerContext.LastVersion))
                return;

            var msg = "This homeserver doesn't support this endpoint.";

            if (clientServerContext.LastVersion != ClientServerApiVersion.Unknown)
                msg +=
                    $"The endpoint was removed in spec version {clientServerContext.LastVersion.ToJsonString()}";
            else
                msg +=
                    $"The endpoint was added in spec version {clientServerContext.MinVersion.ToJsonString()}";

            throw new MatrixException(msg);
        }
    }
}