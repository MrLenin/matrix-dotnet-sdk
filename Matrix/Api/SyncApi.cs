using System;
using System.Threading;
using Matrix.Backends;
using Matrix.Structures;
using Newtonsoft.Json;

namespace Matrix
{
    public partial class MatrixApi
    {
        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "get-matrix-client-r0-sync")]
        public void ClientSync(bool connectionFailureTimeout = false)
        {
            ThrowIfNotSupported();

            var urlString = "/_matrix/client/r0/sync?timeout=" + SyncTimeout;

            if (!string.IsNullOrEmpty(_syncToken))
                urlString += "&since=" + _syncToken;

            var apiPath = new Uri(urlString, UriKind.Relative);
            var error = _matrixApiBackend.HandleGet(apiPath, true, out var response);

            if (error.IsOk)
            {
                try
                {
                    var sync = JsonConvert.DeserializeObject<MatrixSync>(response.ToString(), _eventConverter);
                    ProcessSync(sync);
                    IsConnected = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException);
                    throw new MatrixException("Could not decode sync", e);
                }
            }
            else if (connectionFailureTimeout)
            {
                IsConnected = false;
                Console.Error.WriteLine("Couldn't reach the matrix home server during a sync.");
                Console.Error.WriteLine(error.ToString());
                Thread.Sleep(BadSyncTimeout);
            }

            if (RunningInitialSync)
                RunningInitialSync = false;
        }

        public void StartSyncThreads()
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
                    throw new Exception("Can't start thread, already running");

                _pollThread.Start();
            }
        }

        public void StopSyncThreads()
        {
            _shouldRun = false;
            _pollThread.Join();
        }
    }
}