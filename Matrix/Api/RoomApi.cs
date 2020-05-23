using System;
using System.Threading;
using System.Threading.Tasks;
using Matrix.Backends;
using Matrix.Structures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public partial class MatrixApi : IDisposable
    {
        private readonly Mutex _eventSendMutex = new Mutex();

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "post-matrix-client-r0-rooms-roomid-send")]
        private async Task<string> RoomSend(string roomId, string type, MatrixMRoomMessage msg, string txnId = "")
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/send/{type}/{txnId}", UriKind.Relative);
            var msgData = ObjectToJson(msg);

            var res = await _matrixApiBackend.PutAsync(
                apiPath, true, msgData
            ).ConfigureAwait(false);

            if (!res.Error.IsOk) throw new MatrixException(res.Error.ToString());

            return res.Result["event_id"].ToObject<string>();
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "post-matrix-client-r0-rooms-roomid-leave")]
        public void RoomLeave(string roomId)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/leave", UriKind.Relative);
            var error = _matrixApiBackend.Post(apiPath, true, null, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "put-matrix-client-r0-rooms-roomid-state-eventtype")]
        public virtual string RoomStateSend(string roomId, string type, MatrixRoomStateEvent message, string key = "")
        {
            ThrowIfNotSupported();

            var msgData = ObjectToJson(message);
            var apiPath = new Uri($"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/state/{type}/{key}",
                UriKind.Relative);
            var error = _matrixApiBackend.Put(apiPath, true, msgData, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return result["event_id"].ToObject<string>();
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "post-matrix-client-r0-rooms-roomid-invite")]
        public void InviteToRoom(string roomId, string userId)
        {
            ThrowIfNotSupported();

            var msgData = JObject.FromObject(new {user_id = userId});
            var apiPath = new Uri($"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/invite", UriKind.Relative);
            var error = _matrixApiBackend.Post(apiPath, true, msgData, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "put-matrix-client-r0-rooms-roomid-send-eventtype-txnid")]
        public async Task<string> RoomMessageSend(string roomId, string type, MatrixMRoomMessage message)
        {
            ThrowIfNotSupported();

            if (message.body == null) throw new Exception("Missing body in message");
            if (message.msgtype == null) throw new Exception("Missing msgtype in message");

            var txnId = _rng.Next(int.MinValue, int.MaxValue);
            var msgData = ObjectToJson(message);

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/send/{type}/{txnId}", UriKind.Relative);

            // Send messages in order.
            // XXX: Mutex was removed because it's not task safe, need another mechanism.
            while (true)
            {
                var res = await _matrixApiBackend.PutAsync(
                    apiPath, true, msgData
                ).ConfigureAwait(false);

                if (res.Error.IsOk) return res.Result["event_id"].ToObject<string>();

                if (res.Error.MatrixErrorCode == MatrixErrorCode.LimitExceeded)
                {
                    var backoff = res.Error.RetryAfter != -1 ? res.Error.RetryAfter : 1000;
                    _log.LogWarning($"Sending m{txnId} failed. Will retry in {backoff}ms");
                    await Task.Delay(backoff).ConfigureAwait(false);
                }
                else
                {
                    throw new MatrixException(res.Error.ToString());
                }
            }
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "post-matrix-client-r0-rooms-roomid-receipt-receipttype-eventid")]
        public void RoomTypingSend(string roomId, bool typing, int timeout = 0)
        {
            ThrowIfNotSupported();

            var msgData = timeout == 0 ? JObject.FromObject(new {typing}) : JObject.FromObject(new {typing, timeout});

            var apiPath =
                new Uri(
                    $"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/typing/{Uri.EscapeDataString(UserId)}",
                    UriKind.Relative);

            var error = _matrixApiBackend.Put(apiPath, true, msgData, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "get-matrix-client-r0-rooms-roomid-state")]
        public MatrixEvent[] GetRoomState(string roomId)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/state", UriKind.Relative);
            var error = _matrixApiBackend.Get(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return JsonConvert.DeserializeObject<MatrixEvent[]>(result.ToString(), _eventConverter);
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "get-matrix-client-r0-rooms-roomid-state-eventtype")]
        public MatrixEventContent GetRoomStateType(string roomId, string type)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/state/{type}/", UriKind.Relative);
            var error = _matrixApiBackend.Get(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return _eventConverter.GetContent(result as JObject, new Newtonsoft.Json.JsonSerializer(), type);
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "get-matrix-client-r0-rooms-roomid-messages")]
        public ChunkedMessages GetRoomMessages(string roomId)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/messages?limit=100&dir=b", UriKind.Relative);
            var error = _matrixApiBackend.Get(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return JsonConvert.DeserializeObject<ChunkedMessages>(result.ToString(), _eventConverter);
        }


        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "post-matrix-client-r0-rooms-roomid-join")]
        public string ClientJoin(string roomId)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/join/{Uri.EscapeDataString(roomId)}", UriKind.Relative);
            var error = _matrixApiBackend.Post(apiPath, true, null, out var result);

            if (!error.IsOk) return null;

            roomId = result["room_id"].ToObject<string>();
            return roomId;
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer, "post-matrix-client-r0-createroom")]
        public string ClientCreateRoom(MatrixCreateRoom roomRequest = null)
        {
            ThrowIfNotSupported();

            var req = roomRequest != null ? ObjectToJson(roomRequest) : null;
            var apiPath = new Uri("/_matrix/client/r0/createRoom", UriKind.Relative);
            var error = _matrixApiBackend.Post(apiPath, true, req, out var result);

            if (!error.IsOk) return null;

            var roomid = result["room_id"].ToObject<string>();
            return roomid;
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "get-matrix-client-r0-user-userid-rooms-roomid-tags")]
        public RoomTags RoomGetTags(string roomId)
        {
            ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/user/{UserId}/rooms/{roomId}/tags", UriKind.Relative);
            var error = _matrixApiBackend.Get(apiPath, true, out var result);

            if (!error.IsOk) throw new MatrixException(error.ToString());

            return result.ToObject<RoomTags>();
        }

        [MatrixSpec(EMatrixSpecApiVersion.R001, EMatrixSpecApi.ClientServer,
            "get-matrix-client-r0-user-userid-rooms-roomid-tags")]
        public void RoomPutTag(string roomId, string tag, double order)
        {
            ThrowIfNotSupported();

            var req = new JObject {["order"] = order};
            var apiPath = new Uri($"/_matrix/client/r0/user/{UserId}/rooms/{roomId}/tags/{tag}", UriKind.Relative);
            var error = _matrixApiBackend.Put(apiPath, true, req, out _);

            if (!error.IsOk) throw new MatrixException(error.ToString());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            _eventSendMutex?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}