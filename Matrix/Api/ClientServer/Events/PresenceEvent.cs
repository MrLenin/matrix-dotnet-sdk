using System;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.StateEventContent;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
    public class PresenceEventContent : IEventContent
    {
        [JsonProperty(@"last_active_ago")]
        public long LastActiveAgo { get; }
        [JsonProperty(@"avatar_url")]
        public Uri AvatarUrl { get; }
        [JsonProperty(@"displayname")]
        public string DisplayName { get; }
        [JsonProperty(@"presence")]
        public PresenceStatus PresenceStatus { get; }
        [JsonProperty(@"currently_active")]
        public bool CurrentlyActive { get; }
        [JsonProperty(@"status_msg")]
        public string StatusMessage { get; }
    }

    public class PresenceEvent : IEvent
    {
        [JsonProperty(@"sender")]
        public string Sender { get; }

        [JsonProperty(@"content")]
        public PresenceEventContent Content { get; set; }
        public EventKind EventKind { get; set; }
        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (PresenceEventContent) value;
        }

        public static string ToJsonString()
        {
            return @"m.presence";
        }
    }
}