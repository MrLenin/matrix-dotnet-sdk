using System;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;

namespace Matrix.Api.ClientServer.Events
{
    public class ReceiptEvent : IEvent
    {
        [DataMember(Name = @"room_id")]
        public string RoomId { get; }

        [DataMember(Name = @"content")]
        public ReceiptEventContent Content { get; }
        IEventContent IEvent.Content => Content;
        public EventKind EventKind { get; set; }

        public static string ToJsonString()
        {
            return @"m.receipt";
        }
    }
}
