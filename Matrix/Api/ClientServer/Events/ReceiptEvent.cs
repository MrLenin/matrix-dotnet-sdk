using System;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
    public class ReceiptEvent : IEvent
    {
        [JsonProperty(@"room_id")]
        public string RoomId { get; }

        [JsonProperty(@"content")]
        public ReceiptEventContent Content { get; set; }
        public EventKind EventKind { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (ReceiptEventContent) value;
        }

        public static string ToJsonString()
        {
            return @"m.receipt";
        }
    }
}
