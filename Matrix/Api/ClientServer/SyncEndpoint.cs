using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.Structures;
using Matrix.Api.Versions;
using Matrix.Properties;
using Matrix.Structures;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer
{
    public sealed class SyncResponse : IResponse
    {
        [JsonProperty(@"next_batch")]
        public string NextBatch { get; set; }
        [JsonProperty(@"rooms")]
        public SyncRooms RoomUpdates { get; set; }
        [JsonProperty(@"presence")]
        public PresenceData PresenceUpdates { get; set; }
        [JsonProperty(@"account_data")]
        public AccountData AccountData { get; set; }
        [JsonProperty(@"to_device")]
        public ToDeviceData ToDeviceData { get; set; }
        [JsonProperty(@"device_lists")]
        public DeviceLists DeviceLists { get; set; }
        [JsonProperty(@"device_one_time_keys_count")]
        public IDictionary<string, int> DeviceOneTimeKeysCount { get; set; }

        public ErrorCode? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public long? RetryAfterMilliseconds { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }

    public class SyncEndpoint
    {
        //public event MatrixApiRoomJoinedDelegate OnSyncJoinEvent;
        //public event MatrixApiRoomInviteDelegate OnSyncInviteEvent;


        ///// <summary>
        ///// Timeout in seconds between sync requests.
        ///// </summary>
        public int Timeout { get; set; }

        //public int BadTimeout { get; set; } = 25000;
        //public bool IsConnected { get; private set; }
        //public bool IsInitialSync { get; private set; } = true;

        //private readonly MatrixApi _matrixApi;

        //private bool _shouldRun;
        //private Thread _pollThread;

        //private string _token = "";

        //public SyncEndpoint(MatrixApi matrixApi) =>
        //    _matrixApi = matrixApi;

        //public virtual string Token
        //{
        //    get => _token;
        //    set
        //    {
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            _token = "";
        //            IsInitialSync = true;
        //        }
        //        else
        //        {
        //            _token = value;
        //            IsInitialSync = false;
        //        }
        //    }
        //}

        private readonly Uri _apiPath;
        private readonly MatrixApi _matrixApi;

        public SyncEndpoint(MatrixApi matrixApi, int timeout = 10000)
        {
            Timeout = timeout;
            _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));
            _apiPath = new Uri(@"/_matrix/client/r0/sync", UriKind.Relative);
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-sync")]
        public void Sync(string sinceToken, bool connectionFailureTimeout = false)
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new UriBuilder(_apiPath)
            {
                Query = $@"?timeout={Timeout}{(string.IsNullOrEmpty(sinceToken) ? "" : "&since=")}{sinceToken}"
            };

            var syncResponse = _matrixApi.Backend.Request<SyncResponse>(apiPath.Uri, true);

            //if (syncResponse.ErrorCode == null)
            //    ProcessSync(sync);
        }

        //private void ProcessSync(MatrixSync syncData)
        //{
        //    //Grab data from rooms the user has joined.
        //    foreach (var (roomId, matrixEventRoomJoined) in syncData.Rooms.Join)
        //        OnSyncJoinEvent?.Invoke(roomId, matrixEventRoomJoined);

        //    foreach (var (roomId, matrixEventRoomInvited) in syncData.Rooms.Invite)
        //        OnSyncInviteEvent?.Invoke(roomId, matrixEventRoomInvited);

        //    _token = syncData.NextBatch;
        //}

//        private void PollThread_Run()
//        {
//            while (_shouldRun)
//            {
//                try
//                {
//                    ClientSync(true);
//                }
//                catch (Exception e)
//                {
//#if DEBUG
//                    Console.WriteLine(Resources.SyncMatrixException);
//                    Console.WriteLine(e);
//                    throw;
//#endif
//                }

//                Thread.Sleep(250);
//            }
//        }

//        public void Start()
//        {
//            if (_pollThread == null)
//            {
//                _pollThread = new Thread(PollThread_Run);
//                _pollThread.Start();
//                _shouldRun = true;
//            }
//            else
//            {
//                if (_pollThread.IsAlive)
//                    throw new Exception(Resources.ThreadAlreadyRunning);

//                _pollThread.Start();
//            }
//        }

//        public void Stop()
//        {
//            _shouldRun = false;
//            _pollThread.Join();
//        }
    }
}