using System;
using System.Runtime.Serialization;

using Matrix.Api.ClientServer.Enumerations;

namespace Matrix.Api.ClientServer.Events
{
    public class PresenceEventContent : IEventContent
    {
        [DataMember(Name = @"last_active_ago")]
        public long LastActiveAgo { get; }
        [DataMember(Name = @"avatar_url")]
        public Uri AvatarUrl { get; }
        [DataMember(Name = @"displayname")]
        public string DisplayName { get; }
        [DataMember(Name = @"presence")]
        public PresenceStatus PresenceStatus { get; }
        [DataMember(Name = @"currently_active")]
        public bool CurrentlyActive { get; }
        [DataMember(Name = @"status_msg")]
        public string StatusMessage { get; }
    }

    public class PresenceEvent : IEvent
    {
        [DataMember(Name = @"sender")]
        public string Sender { get; }

        [DataMember(Name = @"content")]
        public PresenceEventContent Content { get; }
        IEventContent IEvent.Content => Content;
        public EventKind EventKind { get; set; }

        public static string ToJsonString()
        {
            return @"m.presence";
        }
    }
}