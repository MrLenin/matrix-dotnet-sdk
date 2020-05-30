using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text.Json;

using Matrix.Abstractions;
using Matrix.Api;
using Matrix.Api.ClientServer;
using Matrix.Api.Versions;
using Matrix.Backends;
using Matrix.Properties;
using Matrix.Structures;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json.Linq;

using static Matrix.Api.SpecificationContext;

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
    public class MatrixApi
    {
        public virtual Uri BaseUrl { get; }
        public virtual string UserId { get; protected set; }
        public IMatrixApiBackend Backend { get; }
        public Random Rng { get; }

        public ILogger Log { get; } = Logger.Factory.CreateLogger<MatrixApi>();

        public RoomApi Room { get; }
        public LoginEndpoint LoginEndpoint { get; }
        public VersionsEndpoint VersionsEndpoint { get; }
        public DeviceApi Device { get; }
        public MediaApi Media { get; }
        public ProfileApi Profile { get; }
        public SyncApi Sync { get; }
        public RoomDirectoryApi RoomDirectory { get; }

        private VersionsContext _versions;
        private readonly bool _isAppservice;
        private AuthenticationContext _currentLogin;

        public MatrixApi(Uri url)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException(Resources.InvalidUrl);

            _isAppservice = false;
            Backend = new HttpBackend(url, UserId);
            BaseUrl = url;
            Rng = new Random(DateTime.Now.Millisecond);

            Room = new RoomApi(this);
            LoginEndpoint = new LoginEndpoint(this);
            VersionsEndpoint = new VersionsEndpoint(this);
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

            Room = new RoomApi(this);
            LoginEndpoint = new LoginEndpoint(this);
            VersionsEndpoint = new VersionsEndpoint(this);
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

            Room = new RoomApi(this);
            LoginEndpoint = new LoginEndpoint(this);
            VersionsEndpoint = new VersionsEndpoint(this);
            Device = new DeviceApi(this);
            Media = new MediaApi(this);
            Profile = new ProfileApi(this);
            Sync = new SyncApi(this);
            RoomDirectory = new RoomDirectoryApi(this);
        }

        public virtual string GetAccessToken()
        {
            return _currentLogin.AccessToken;
        }

        public virtual AuthenticationContext GetCurrentLogin()
        {
            return _currentLogin;
        }

        public void SetLogin(AuthenticationContext response)
        {
            _currentLogin = response ?? throw new ArgumentNullException(nameof(response));
            UserId = response.UserId;
            Backend.SetAccessToken(response.AccessToken);
        }

        public bool IsLoggedIn()
        {
            //TODO: Check token is still valid
            return _currentLogin != null;
        }



        [MatrixSpec(ClientServerApiVersion.R040, "get-matrix-client-r0-joined_rooms")]
        public List<string> GetJoinedRooms()
        {
            throw new NotImplementedException();
//             var apiPath = new Uri("/_matrix/client/r0/joined_rooms", UriKind.Relative);
//             var error = Backend.HandleGet(apiPath, true, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return (result as JToken)?.GetValue("joined_rooms", StringComparison.InvariantCulture)
//                 .ToObject<List<string>>();
        }

        [MatrixSpec(ClientServerApiVersion.R040, "get-matrix-client-r0-joined_members")]
        public Dictionary<string, MatrixProfile> GetJoinedMembers(string roomId)
        {
            throw new NotImplementedException();
//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/joined_members", UriKind.Relative);
//             var error = Backend.HandleGet(apiPath, true, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return (result as JsonDocument)?.GetValue("joined", StringComparison.InvariantCulture)
//                 .ToObject<Dictionary<string, MatrixProfile>>();
        }

        public void RegisterUserAsAppservice(string user)
        {
            if (!_isAppservice)
                throw new MatrixException(Resources.AppserviceNotRegistered);
            throw new NotImplementedException();
// 
//             var request = JToken.FromObject(
//                 new
//                 {
//                     type = "m.login.application_service",
//                     user
//                 });
// 
//             var apiPath = new Uri("/_matrix/client/r0/register", UriKind.Relative);
//             var error = Backend.HandlePost(apiPath, true, request, out _);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        public void ThrowIfNotSupported([CallerMemberName] string? name = null)
        {
            if (name == null) return;

            _versions ??= VersionsEndpoint.RequestVersions();

            if (!((typeof(MatrixApi).GetMethod(name) ?? throw new InvalidOperationException()).GetCustomAttribute(
                typeof(MatrixSpecAttribute)) is MatrixSpecAttribute spec))
            {
#if DEBUG
                Log.LogWarning($"{name} has no MatrixSpec attribute, cannot determine homeserver support");
#endif
                return;
            }

            var clientServerContext = spec.SpecificationContext.Specification switch
                {
                Specification.ClientServer => ClientServer(spec.SpecificationContext)
                };
            
            // Ensure we support a version of the spec >= the min version and <= the last version.
            if (!_versions.Versions
                .Any(version => version >= clientServerContext.AddedVersion && version <= clientServerContext.RemovedVersion))
                return;

            var msg = "This homeserver doesn't support this endpoint.";

            if (clientServerContext.RemovedVersion != ClientServerApiVersion.Unknown)
                msg +=
                    $"The endpoint was removed in spec version {clientServerContext.RemovedVersion.ToJsonString()}";
            else
                msg +=
                    $"The endpoint was added in spec version {clientServerContext.AddedVersion.ToJsonString()}";

            throw new MatrixException(msg);
        }
    }
}