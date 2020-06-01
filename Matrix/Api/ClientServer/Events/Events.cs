using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using JsonSubTypes;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.EventContent;
using Matrix.Api.ClientServer.Events;
using Matrix.Api.ClientServer.RoomContent;
using Matrix.Api.ClientServer.StateContent;
using Matrix.Api.ClientServer.Structures;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
    public interface IEventContent
    {
    }

    public interface IRoomContent : IEventContent
    {
    }

    public interface IMessageContent : IRoomContent
    {
        [JsonProperty(@"body")] string MessageBody { get; set; }
        [JsonProperty(@"msgtype")] MessageKind MessageKind { get; }
    }

    public interface IStateContent : IRoomContent
    {
    }

    public interface IEvent
    {
        IEventContent Content { get; set; }

        [JsonProperty(@"type")] EventKind EventKind { get; set; }
    }

    public interface IRoomEvent<T> : IEvent
        where T : class, IRoomContent
    {
        [JsonProperty(@"event_id")] string EventId { get; set; }
        [JsonProperty(@"sender")] string Sender { get; set; }
        [JsonProperty(@"origin_server_ts")] long OriginServerTimestamp { get; set; }
        [JsonProperty(@"unsigned")] UnsignedData UnsignedData { get; set; }
        [JsonProperty(@"room_id")] string RoomId { get; set; }

        [JsonProperty(@"content")] new T Content { get; set; }
    }

    public interface IStateEvent<T> : IRoomEvent<T>
        where T : class, IStateContent
    {
        [JsonProperty(@"state_key")] string StateKey { get; set; }
        [JsonProperty(@"prev_content")] T? PrevContent { get; set; }
    }

    public class RoomEvent<TRoomEventContent> : IRoomEvent<TRoomEventContent>
        where TRoomEventContent : class, IRoomContent
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
        where TStateEventContent : class, IStateContent
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

    public sealed class RoomEvent : RoomEvent<IRoomContent>
    { }

    public sealed class StateEvent : StateEvent<IStateContent>
    { }

    public class PresenceEvent : IEvent
    {
        [JsonProperty(@"sender")]
        public string Sender { get; }

        [JsonProperty(@"content")]
        public PresenceContent Content { get; set; }
        public EventKind EventKind { get; set; }
        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (PresenceContent)value;
        }

        public static string ToJsonString()
        {
            return @"m.presence";
        }
    }

    public sealed class ReceiptEvent : IEvent
    {
        [JsonProperty(@"room_id")]
        public string RoomId { get; }

        [JsonProperty(@"content")]
        public ReceiptContent Content { get; set; }
        public EventKind EventKind { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (ReceiptContent)value;
        }

        public static string ToJsonString()
        {
            return @"m.receipt";
        }
    }

    public sealed class RedactionRoomEvent : IRoomEvent<RedactionContent>
    {
        [JsonProperty(@"redacts")]
        public string RedactedEventId { get; set; }

        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }
        public RedactionContent Content { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (RedactionContent) value;
        }

        public EventKind EventKind { get; set; }
    }

    public sealed class MessageRoomEvent : IRoomEvent<IMessageContent>
    {
        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }


        public IMessageContent Content { get; set; }

        IMessageContent IRoomEvent<IMessageContent>.Content
        {
            get => Content;
            set => Content = value;
        }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (IMessageContent) value;
        }

        public EventKind EventKind { get; set; }
    }
}
