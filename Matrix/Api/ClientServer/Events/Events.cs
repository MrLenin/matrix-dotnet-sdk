using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JsonSubTypes;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomEventContent;
using Matrix.Api.ClientServer.StateEventContent;
using Matrix.Api.ClientServer.Structures;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
    public interface IEventContent
    {
    }

    public interface IRoomEventContent : IEventContent
    {
    }

    public interface IMessageEventContent : IRoomEventContent
    {
        [JsonProperty(@"body")] string MessageBody { get; set; }
        [JsonProperty(@"msgtype")] MessageKind MessageKind { get; }
    }

    public interface IStateEventContent : IRoomEventContent
    {
    }

    public interface IEvent
    {
        IEventContent Content { get; set; }

        [JsonProperty(@"type")] EventKind EventKind { get; set; }
    }

    public interface IRoomEvent<T> : IEvent
        where T : class, IRoomEventContent
    {
        [JsonProperty(@"event_id")] string EventId { get; set; }
        [JsonProperty(@"sender")] string Sender { get; set; }
        [JsonProperty(@"origin_server_ts")] long OriginServerTimestamp { get; set; }
        [JsonProperty(@"unsigned")] UnsignedData UnsignedData { get; set; }
        [JsonProperty(@"room_id")] string RoomId { get; set; }

        [JsonProperty(@"content")] new T Content { get; set; }
    }

    public interface IStateEvent<T> : IRoomEvent<T>
        where T : class, IStateEventContent
    {
        [JsonProperty(@"state_key")] string StateKey { get; set; }
        [JsonProperty(@"prev_content")] T? PrevContent { get; set; }
    }

    public class RoomEvent<TRoomEventContent> : IRoomEvent<TRoomEventContent>
        where TRoomEventContent : class, IRoomEventContent
    {
        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }

        public static implicit operator RoomEvent<TRoomEventContent>(MessageRoomEvent messageEvent) => new RoomEvent<TRoomEventContent>(messageEvent);

        public RoomEvent() {}

        private RoomEvent(MessageRoomEvent roomEvent)
        {
            if (roomEvent == null) throw new ArgumentNullException(nameof(roomEvent));
            if (roomEvent.Content.GetType() != typeof(TRoomEventContent))
                throw new ArgumentException($@"Argument does not wrap correct type '{typeof(TRoomEventContent)}'", nameof(roomEvent));

            EventKind = roomEvent.EventKind;
            EventId = roomEvent.EventId;
            Sender = roomEvent.Sender;
            OriginServerTimestamp = roomEvent.OriginServerTimestamp;
            UnsignedData = roomEvent.UnsignedData;
            RoomId = roomEvent.RoomId;
            Content = (TRoomEventContent)roomEvent.Content;
        }

        public TRoomEventContent Content { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (TRoomEventContent) value;
        }

        public EventKind EventKind { get; set; }
    }

    public class StateEvent<TStateEventContent> : IStateEvent<TStateEventContent>
        where TStateEventContent : class, IStateEventContent
    {
        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }
        public string StateKey { get; set; }
        public TStateEventContent? PrevContent { get; set; }

        public TStateEventContent Content { get; set; }

        TStateEventContent IRoomEvent<TStateEventContent>.Content
        {
            get => Content;
            set => Content = value;
        }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (TStateEventContent) value;
        }

        public EventKind EventKind { get; set; }
    }

    public sealed class RoomEvent : RoomEvent<IRoomEventContent>
    { }

    public sealed class StateEvent : StateEvent<IStateEventContent>
    { }

    public sealed class RedactionRoomEvent : IRoomEvent<RedactionEventContent>
    {
        [JsonProperty(@"redacts")]
        public string RedactedEventId { get; set; }

        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }
        public RedactionEventContent Content { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (RedactionEventContent) value;
        }

        public EventKind EventKind { get; set; }
    }

    public sealed class MessageRoomEvent : IRoomEvent<IMessageEventContent>
    {
        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }


        public IMessageEventContent Content { get; set; }

        IMessageEventContent IRoomEvent<IMessageEventContent>.Content
        {
            get => Content;
            set => Content = value;
        }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (IMessageEventContent) value;
        }

        public EventKind EventKind { get; set; }
    }
}
