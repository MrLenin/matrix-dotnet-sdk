using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Matrix.Backends;
using Matrix.Structures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

/**
 * This class contains all the methods needed to call the Matrix C2S API. The methods are split into files
 * inside ./Api.
 */

namespace Matrix
{
    public delegate void MatrixApiRoomJoinedDelegate(string roomid, MatrixEventRoomJoined joined);

    public delegate void MatrixApiRoomInviteDelegate(string roomid, MatrixEventRoomInvited invited);

    // We need to mock MatrixAPI, hence needing virtuals.
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public partial class MatrixApi
    {
        public event MatrixApiRoomJoinedDelegate OnSyncJoinEvent;
        public event MatrixApiRoomInviteDelegate OnSyncInviteEvent;

        public bool IsConnected { get; private set; }
        public virtual bool RunningInitialSync { get; private set; } = true;
        public virtual Uri BaseUrl { get; private set; }
        public int BadSyncTimeout { get; set; } = 25000;
        public virtual string UserId { get; set; }

        private MatrixVersions _versions;
        private string _syncToken = "";
        private readonly ILogger _log = Logger.Factory.CreateLogger<MatrixApi>();
        private readonly bool _isAppservice;
        private MatrixLoginResponse _currentLogin;
        private Thread _pollThread;
        private bool _shouldRun;
        private readonly Random _rng;
        private readonly JSONEventConverter _eventConverter;
        private readonly IMatrixApiBackend _matrixApiBackend;

        private static readonly JsonSerializer MatrixSerializer = new JsonSerializer();

        /// <summary>
        /// Timeout in seconds between sync requests.
        /// </summary>
        public int SyncTimeout { get; set; } = 10000;

        public MatrixApi(Uri url)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException("URL is not valid");

            _isAppservice = false;
            _matrixApiBackend = new HttpBackend(url);
            BaseUrl = url;
            _rng = new Random(DateTime.Now.Millisecond);
            _eventConverter = new JSONEventConverter();
        }

        public MatrixApi(Uri url, string applicationToken, string userId)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException("URL is not valid");

            _isAppservice = true;
            _matrixApiBackend = new HttpBackend(url, userId);
            _matrixApiBackend.SetAccessToken(applicationToken);
            UserId = userId;
            BaseUrl = url;
            _rng = new Random(DateTime.Now.Millisecond);
            _eventConverter = new JSONEventConverter();
        }

        public MatrixApi(Uri url, IMatrixApiBackend backend)
        {
            if (url != null && url.IsWellFormedOriginalString() && !url.IsAbsoluteUri)
                throw new MatrixException("URL is not valid");

            _isAppservice = true;
            _matrixApiBackend = backend;
            BaseUrl = url;
            _rng = new Random(DateTime.Now.Millisecond);
            _eventConverter = new JSONEventConverter();
        }


        public void AddMessageType(string name, Type type)
        {
            _eventConverter.AddMessageType(name, type);
        }

        public void AddEventType(string messageType, Type type)
        {
            _eventConverter.AddEventType(messageType, type);
        }

        private void PollThread_Run()
        {
            while (_shouldRun)
            {
                try
                {
                    ClientSync(true);
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.WriteLine("[warn] A Matrix exception occured during sync!");
                    Console.WriteLine(e);
                    throw;
#endif
                }

                Thread.Sleep(250);
            }
        }

        public void SetSyncToken(string syncToken)
        {
            _syncToken = syncToken;
            RunningInitialSync = false;
        }

        public virtual string GetSyncToken()
        {
            return _syncToken;
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
            _matrixApiBackend.SetAccessToken(response.AccessToken);
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
                throw new Exception("Couldn't convert obj to JSON", e);
            }

            return container;
        }

        public bool IsLoggedIn()
        {
            //TODO: Check token is still valid
            return _currentLogin != null;
        }

        private void ProcessSync(MatrixSync syncData)
        {
            //Grab data from rooms the user has joined.
            foreach (var (roomId, matrixEventRoomJoined) in syncData.Rooms.Join)
                OnSyncJoinEvent?.Invoke(roomId, matrixEventRoomJoined);

            foreach (var (roomId, matrixEventRoomInvited) in syncData.Rooms.Invite)
                OnSyncInviteEvent?.Invoke(roomId, matrixEventRoomInvited);

            _syncToken = syncData.NextBatch;
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "get-matrix-client-versions")]
        public MatrixVersions ClientVersions()
        {
            var apiPath = new Uri("/_matrix/client/versions", UriKind.Relative);
            var error = _matrixApiBackend.HandleGet(apiPath, false, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            var res = result.ToObject<MatrixVersions>();
            _versions = res;
            return res;
        }

        [MatrixSpec(EMatrixSpecApiVersion.R040, EMatrixSpecApi.ClientServer, "get-matrix-client-r0-joined_rooms")]
        public List<string> GetJoinedRooms()
        {
            var apiPath = new Uri("/_matrix/client/r0/joined_rooms", UriKind.Relative);
            var error = _matrixApiBackend.HandleGet(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return (result as JObject)?.GetValue("joined_rooms", StringComparison.InvariantCulture)
                .ToObject<List<string>>();
        }

        [MatrixSpec(EMatrixSpecApiVersion.R040, EMatrixSpecApi.ClientServer, "get-matrix-client-r0-joined_members")]
        public Dictionary<string, MatrixProfile> GetJoinedMembers(string roomId)
        {
            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/joined_members", UriKind.Relative);
            var error = _matrixApiBackend.HandleGet(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return (result as JObject)?.GetValue("joined", StringComparison.InvariantCulture)
                .ToObject<Dictionary<string, MatrixProfile>>();
        }

        public void RegisterUserAsAppservice(string user)
        {
            if (!_isAppservice)
                throw new MatrixException(
                    "This client is not registered as a application service client. You can't create new appservice users");

            var request = JObject.FromObject(
                new
                {
                    type = "m.login.application_service",
                    user
                });

            var apiPath = new Uri("/_matrix/client/r0/register", UriKind.Relative);
            var error = _matrixApiBackend.HandlePost(apiPath, true, request, out _);

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
                _log.LogWarning($"{name} has no MatrixSpec attribute, cannot determine homeserver support");
#endif
                return;
            }

            // Ensure we support a version of the spec >= the min version and <= the last version.
            if (!_versions.SupportedVersions()
                .Any(version => version >= spec.MinVersion && version <= spec.LastVersion))
                return;

            var msg = "This homeserver doesn't support this endpoint.";

            if (spec.LastVersion != EMatrixSpecApiVersion.Unknown)
                msg +=
                    $"The endpoint was removed in spec version {MatrixSpecAttribute.GetStringForVersion(spec.LastVersion)}";
            else
                msg +=
                    $"The endpoint was added in spec version {MatrixSpecAttribute.GetStringForVersion(spec.MinVersion)}";

            throw new MatrixException(msg);
        }
    }
}