using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JsonSubTypes;

using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.Events.RoomContent;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
    public enum EventKind
    {
        Presence,
        Receipt,
        RoomAvatar,
        RoomCanonicalAlias,
        RoomCreate,
        RoomGuestAccess,
        RoomHistoryVisibility,
        RoomJoinRules,
        RoomMembership,
        RoomMessage,
        RoomName,
        RoomPinnedEvents,
        RoomPowerLevels,
        RoomRedaction,
        RoomServerAcl,
        RoomThirdPartyInvite,
        RoomTombstone,
        RoomTopic,
        Typing
    }

    public static class EventKindExtensions
    {
        public static string ToJsonString(this EventKind eventKind)
        {
            return eventKind switch
            {
                EventKind.Presence => @"m.presence",
                EventKind.Receipt => @"m.receipt",
                EventKind.RoomMessage => @"m.room.message",
                EventKind.RoomMembership => @"m.room.member",
                EventKind.RoomCreate => @"m.room.create",
                EventKind.RoomJoinRules => @"m.room.join_rules",
                //EventKind.RoomAliases => @"m.room.aliases",
                EventKind.RoomCanonicalAlias => @"m.room.canonical_alias",
                EventKind.RoomName => @"m.room.name",
                EventKind.RoomTopic => @"m.room.topic",
                EventKind.RoomPowerLevels => @"m.room.power_levels",
                EventKind.RoomHistoryVisibility => @"m.room.history_visibility",
                EventKind.Typing => @"m.typing",
                _ => throw new System.NotImplementedException()
            };
        }

        public static EventKind ToEventType(this string eventType)
        {
            return eventType switch
            {
                @"m.presence" => EventKind.Presence,
                @"m.receipt" => EventKind.Receipt,
                @"m.room.message" => EventKind.RoomMessage,
                @"m.room.member" => EventKind.RoomMembership,
                @"m.room.create" => EventKind.RoomCreate,
                @"m.room.join_rules" => EventKind.RoomJoinRules,
                //@"m.room.aliases" => EventKind.RoomAliases,
                @"m.room.canonical_alias" => EventKind.RoomCanonicalAlias,
                @"m.room.name" => EventKind.RoomName,
                @"m.room.topic" => EventKind.RoomTopic,
                @"m.room.power_levels" => EventKind.RoomPowerLevels,
                @"m.room.history_visibility" => EventKind.RoomHistoryVisibility,
                @"m.typing" => EventKind.Typing,
                _ => throw new System.NotImplementedException()
            };
        }
    }

    public interface IEventContent
    {

    }

    public interface IRoomEventContent : IEventContent
    {

    }

    public interface IRoomMessageEventContent : IRoomEventContent
    {
        [DataMember(Name = @"body")]
        string MessageBody { get; set; }
        [DataMember(Name = @"msgtype")]
        MessageKind MessageKind { get; }
    }

    public interface IRoomStateEventContent : IRoomEventContent
    {

    }

    public interface IEvent
    {
        IEventContent Content { get; }
        [DataMember(Name = @"type")]
        EventKind EventKind { get; set; }
    }

    public class RoomEvent : IEvent
    {
        [DataMember(Name = @"event_id")]
        public string EventId { get; }
        [DataMember(Name = @"sender")]
        public string Sender { get; }
        [DataMember(Name = @"origin_server_ts")]
        public int OriginServerTimestamp { get; }
        [DataMember(Name = @"unsigned")]
        public object UnsignedData { get; }
        [DataMember(Name = @"room_id")]
        public string RoomId { get; }

        [DataMember(Name = @"content")]
        public virtual IRoomEventContent Content { get; }
        IEventContent IEvent.Content => (IEventContent)Content;
        public EventKind EventKind { get; set; }
    }

    public class RoomStateEvent : RoomEvent
    {
        [DataMember(Name = @"state_key")]
        public string StateKey { get; }
        [DataMember(Name = @"prev_content")]
        public IRoomStateEventContent? PrevContent { get; }

        [DataMember(Name = @"content")]
        public new IRoomStateEventContent Content { get; set; }
    }


    public class RoomRedactionEvent : RoomEvent
    {
        [DataMember(Name = @"redacts")]
        public string RedactedEventId { get; set; }
    }
}