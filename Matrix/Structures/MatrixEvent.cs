using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Matrix.Api.ClientServer.Events;

namespace Matrix.Structures
{
    public class MatrixEvent
    {
        /// <summary>
        /// Following http://matrix.org/docs/spec/r0.0.1/client_server.html#get-matrix-client-r0-sync
        /// </summary>
        public MatrixEventContent Content { get; set; }

        public long OriginServerTimeStamp { get; set; }
        public long Age { get; set; }
        public string Sender { get; set; }
        public string Type { get; set; }
        public string EventId { get; set; }
        public string RoomId { get; set; }
        public MatrixEventUnsigned EventUnsigned { get; set; }
        public string StateKey { get; set; }

        // Special case for https://matrix.org/docs/spec/r0.0.1/client_server.html#m-room-member
        public IEnumerable<MatrixStrippedState> InviteRoomState { get; set; }

        public override string ToString()
        {
            var str = typeof(MatrixEvent).GetProperties().Aggregate("Event {",
                (current, prop) => current + ("   " + (prop.Name + ": " + prop.GetValue(this))));
            str += "}";
            return str;
        }
    }

    public class MatrixEventUnsigned
    {
        public MatrixEventUnsigned PrevContent { get; set; }
        public long Age { get; set; }
        public string TransactionId { get; set; }
    }

    /// <summary>
    /// Base content class.
    /// </summary>
    public class MatrixEventContent
    {
        public JsonDocument MxContent { get; set; } = null;
    }

    public class MatrixTimeline
    {
        public bool Limited { get; set; }
        public string PrevBatch { get; set; }
        public IEnumerable<IRoomEvent> Events { get; set; }
    }

    public static class MatrixEventType
    {
        public const string RoomMember = "m.room.member";
    }
}