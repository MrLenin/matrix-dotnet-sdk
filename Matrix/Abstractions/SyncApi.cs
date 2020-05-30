using System;
using System.Threading;

using Matrix.Api.Versions;
using Matrix.Properties;
using Matrix.Structures;

namespace Matrix.Abstractions
{
    public class SyncApi
    {
        public event MatrixApiRoomJoinedDelegate OnSyncJoinEvent;
        public event MatrixApiRoomInviteDelegate OnSyncInviteEvent;

        /// <summary>
        /// Timeout in seconds between sync requests.
        /// </summary>
        public int Timeout { get; set; } = 10000;
        public int BadTimeout { get; set; } = 25000;
        public bool IsConnected { get; private set; }
        public bool IsInitialSync { get; private set; } = true;

        private readonly MatrixApi _matrixApi;

        private bool _shouldRun;
        private Thread _pollThread;

        private string _token = "";

        public SyncApi(MatrixApi matrixApi) =>
            _matrixApi = matrixApi;

        public string Token
        {
            get => _token;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _token = "";
                    IsInitialSync = true;
                }
                else
                {
                    _token = value;
                    IsInitialSync = false;
                }
            }
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-sync")]
        public void ClientSync(bool connectionFailureTimeout = false)
        {
            _matrixApi.ThrowIfNotSupported();

//             var urlString = "/_matrix/client/r0/sync?timeout=" + Timeout;
// 
//             if (!string.IsNullOrEmpty(_token))
//                 urlString += "&since=" + _token;
// 
//             var apiPath = new Uri(urlString, UriKind.Relative);
//             var error = _matrixApi.Backend.HandleGet(apiPath, true, out var response);
// 
//             if (error.IsOk)
//             {
//                 try
//                 {
//                     var sync = JsonConvert.DeserializeObject<MatrixSync>(response.ToString(), _matrixApi.EventConverter);
//                     ProcessSync(sync);
//                     IsConnected = true;
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine(e.InnerException);
//                     throw new MatrixException(Resources.SyncDecodeFail, e);
//                 }
//             }
//             else if (connectionFailureTimeout)
//             {
//                 IsConnected = false;
//                 Console.Error.WriteLine("Couldn't reach the matrix home server during a sync.");
//                 Console.Error.WriteLine(error.ToString());
//                 Thread.Sleep(BadTimeout);
//             }
// 
//             if (IsInitialSync)
//                 IsInitialSync = false;
        }

        private void ProcessSync(MatrixSync syncData)
        {
            //Grab data from rooms the user has joined.
            foreach (var (roomId, matrixEventRoomJoined) in syncData.Rooms.Join)
                OnSyncJoinEvent?.Invoke(roomId, matrixEventRoomJoined);

            foreach (var (roomId, matrixEventRoomInvited) in syncData.Rooms.Invite)
                OnSyncInviteEvent?.Invoke(roomId, matrixEventRoomInvited);

            _token = syncData.NextBatch;
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
                    Console.WriteLine(Resources.SyncMatrixException);
                    Console.WriteLine(e);
                    throw;
#endif
                }

                Thread.Sleep(250);
            }
        }

        public void Start()
        {
            if (_pollThread == null)
            {
                _pollThread = new Thread(PollThread_Run);
                _pollThread.Start();
                _shouldRun = true;
            }
            else
            {
                if (_pollThread.IsAlive)
                    throw new Exception(Resources.ThreadAlreadyRunning);

                _pollThread.Start();
            }
        }

        public void Stop()
        {
            _shouldRun = false;
            _pollThread.Join();
        }
    }
}