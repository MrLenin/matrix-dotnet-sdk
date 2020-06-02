using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Matrix.Api;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.Versions;
using Matrix.Backends;
using Matrix.Properties;
using Matrix.Structures;

using Microsoft.Extensions.Logging;

namespace Matrix.Abstractions
{
    public class RoomApi : IDisposable
    {
        private readonly MatrixApi _matrixApi;
        private readonly Mutex _eventSendMutex = new Mutex();

        public RoomApi(MatrixApi matrixApi) => _matrixApi = matrixApi ?? throw new ArgumentNullException(nameof(matrixApi));

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-rooms-roomid-send")]
        private async Task<string> Send(string roomId, string type, MatrixMRoomMessage msg, string txnId = "")
        {
            _matrixApi.ThrowIfNotSupported();

            var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/send/{type}/{txnId}", UriKind.Relative);
//             var msgData = MatrixApi.ObjectToJson(msg);
// 
//             var res = await _matrixApi.Backend.HandlePutAsync(
//                 apiPath, true, msgData
//             ).ConfigureAwait(false);
// 
//             if (!res.Error.IsOk) throw new MatrixException(res.Error.ToString());
// 
//             return res.Result["event_id"].ToObject<string>();
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-rooms-roomid-leave")]
        public void Leave(string roomId)
        {
            _matrixApi.ThrowIfNotSupported();

//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/leave", UriKind.Relative);
//             var error = _matrixApi.Backend.HandlePost(apiPath, true, null, out _);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "put-matrix-client-r0-rooms-roomid-state-eventtype")]
        public virtual string SendState<T>(string roomId, string type, T message, string key = "")
            where T : class, IStateContent
        {
            _matrixApi.ThrowIfNotSupported();

//             var msgData = MatrixApi.ObjectToJson(message);
//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/state/{type}/{key}",
//                 UriKind.Relative);
//             var error = _matrixApi.Backend.HandlePut(apiPath, true, msgData, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return result["event_id"].ToObject<string>();
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-rooms-roomid-invite")]
        public void InviteTo(string roomId, string userId)
        {
            _matrixApi.ThrowIfNotSupported();

//             var msgData = JObject.FromObject(new {user_id = userId});
//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/invite", UriKind.Relative);
//             var error = _matrixApi.Backend.HandlePost(apiPath, true, msgData, out _);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "put-matrix-client-r0-rooms-roomid-send-eventtype-txnid")]
        public async Task<string> SendMessage(string roomId, string type, MatrixMRoomMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            _matrixApi.ThrowIfNotSupported();

//             if (message.Body == null) throw new Exception(Resources.MessageBodyMissing);
//             if (message.MessageType == null) throw new Exception(Resources.MessageTypeMissing);
// 
//             var txnId = _matrixApi.Rng.Next(int.MinValue, int.MaxValue);
//             var msgData = MatrixApi.ObjectToJson(message);
// 
//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/send/{type}/{txnId}", UriKind.Relative);
// 
//             // Send messages in order.
//             // XXX: Mutex was removed because it's not task safe, need another mechanism.
//             while (true)
//             {
//                 var res = await _matrixApi.Backend.HandlePutAsync(
//                     apiPath, true, msgData
//                 ).ConfigureAwait(false);
// 
//                 if (res.Error.IsOk) return res.Result["event_id"].ToObject<string>();
// 
//                 if (res.Error.MatrixErrorCode == ErrorCode.LimitExceeded)
//                 {
//                     var backoff = res.Error.RetryAfter != -1 ? res.Error.RetryAfter : 1000;
//                     _matrixApi.Log.LogWarning($"Sending m{txnId} failed. Will retry in {backoff}ms");
//                     await Task.Delay(backoff).ConfigureAwait(false);
//                 }
//                 else
//                 {
//                     throw new MatrixException(res.Error.ToString());
//                 }
//             }
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-rooms-roomid-receipt-receipttype-eventid")]
        public void SendTyping(string roomId, bool typing, int timeout = 0)
        {
            _matrixApi.ThrowIfNotSupported();

//             var msgData = timeout == 0 ? JObject.FromObject(new {typing}) : JObject.FromObject(new {typing, timeout});
// 
//             var apiPath =
//                 new Uri(
//                     $"/_matrix/client/r0/rooms/{Uri.EscapeDataString(roomId)}/typing/{Uri.EscapeDataString(_matrixApi.UserId)}",
//                     UriKind.Relative);
// 
//             var error = _matrixApi.Backend.HandlePut(apiPath, true, msgData, out _);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-rooms-roomid-state")]
        public IEnumerable<IStateEvent> GetState(string roomId)
        {
            _matrixApi.ThrowIfNotSupported();

//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/state", UriKind.Relative);
//             var error = _matrixApi.Backend.HandleGet(apiPath, true, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return JsonConvert.DeserializeObject<MatrixEvent[]>(result.ToString(), _matrixApi.EventConverter);
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-rooms-roomid-state-eventtype")]
        public IStateContent GetStateType(string roomId, string type)
        {
            _matrixApi.ThrowIfNotSupported();

//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/state/{type}/", UriKind.Relative);
//             var error = _matrixApi.Backend.HandleGet(apiPath, true, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return _matrixApi.EventConverter.GetContent(result as JObject, type);
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-rooms-roomid-messages")]
        public ChunkedMessages GetMessages(string roomId)
        {
            _matrixApi.ThrowIfNotSupported();

//             var apiPath = new Uri($"/_matrix/client/r0/rooms/{roomId}/messages?limit=100&dir=b", UriKind.Relative);
//             var error = _matrixApi.Backend.HandleGet(apiPath, true, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return JsonConvert.DeserializeObject<ChunkedMessages>(result.ToString(), _matrixApi.EventConverter);
            throw new NotImplementedException();
        }


        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-rooms-roomid-join")]
        public string ClientJoin(string roomId)
        {
            _matrixApi.ThrowIfNotSupported();

//             var apiPath = new Uri($"/_matrix/client/r0/join/{Uri.EscapeDataString(roomId)}", UriKind.Relative);
//             var error = _matrixApi.Backend.HandlePost(apiPath, true, null, out var result);
// 
//             if (!error.IsOk) return null;
// 
//             roomId = result["room_id"].ToObject<string>();
//             return roomId;
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "post-matrix-client-r0-createroom")]
        public string ClientCreate(MatrixCreateRoom roomRequest = null)
        {
            _matrixApi.ThrowIfNotSupported();

//             var req = roomRequest != null ? MatrixApi.ObjectToJson(roomRequest) : null;
//             var apiPath = new Uri("/_matrix/client/r0/createRoom", UriKind.Relative);
//             var error = _matrixApi.Backend.HandlePost(apiPath, true, req, out var result);
// 
//             if (!error.IsOk) return null;
// 
//             var roomid = result["room_id"].ToObject<string>();
//             return roomid;
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-user-userid-rooms-roomid-tags")]
        public RoomTags GetTags(string roomId)
        {
            _matrixApi.ThrowIfNotSupported();

//             var apiPath = new Uri($"/_matrix/client/r0/user/{_matrixApi.UserId}/rooms/{roomId}/tags", UriKind.Relative);
//             var error = _matrixApi.Backend.HandleGet(apiPath, true, out var result);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
// 
//             return result.ToObject<RoomTags>();
            throw new NotImplementedException();
        }

        [MatrixSpec(ClientServerVersion.R001, "get-matrix-client-r0-user-userid-rooms-roomid-tags")]
        public void PutTag(string roomId, string tag, double order)
        {
            _matrixApi.ThrowIfNotSupported();

//             var req = new JObject {["order"] = order};
//             var apiPath = new Uri($"/_matrix/client/r0/user/{_matrixApi.UserId}/rooms/{roomId}/tags/{tag}", UriKind.Relative);
//             var error = _matrixApi.Backend.HandlePut(apiPath, true, req, out _);
// 
//             if (!error.IsOk) throw new MatrixException(error.ToString());
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