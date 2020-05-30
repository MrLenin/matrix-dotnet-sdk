using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JsonSubTypes;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomEventContent;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
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

    public interface IStateEventContent : IRoomEventContent
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

    public class StateEvent : RoomEvent
    {
        [DataMember(Name = @"state_key")]
        public string StateKey { get; }
        [DataMember(Name = @"prev_content")]
        public IStateEventContent? PrevContent { get; }

        [DataMember(Name = @"content")]
        public new IStateEventContent Content { get; set; }
    }


    public class RoomRedactionEvent : RoomEvent
    {
        [DataMember(Name = @"redacts")]
        public string RedactedEventId { get; set; }
    }
}