using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.ClientServer.EventContent;
using Matrix.Api.ClientServer.RoomContent;
using Matrix.Api.ClientServer.Structures;

using Newtonsoft.Json;

namespace Matrix.Api.ClientServer.Events
{
    public interface IEventContent
    {
    }

    public interface IRoomContent
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

    public interface IRoomEvent
    {
        [JsonProperty(@"event_id")] string EventId { get; set; }
        [JsonProperty(@"sender")] string Sender { get; set; }
        [JsonProperty(@"origin_server_ts")] long OriginServerTimestamp { get; set; }
        [JsonProperty(@"unsigned")] UnsignedData UnsignedData { get; set; }
        [JsonProperty(@"room_id")] string RoomId { get; set; }
        
        [JsonProperty(@"content")]
        IRoomContent Content { get; set; }

        [JsonProperty(@"type")] EventKind EventKind { get; set; }
    }

    public interface IStateEvent : IRoomEvent
    {
        [JsonProperty(@"state_key")] string StateKey { get; set; }
        [JsonProperty(@"prev_content")] IStateContent? PrevContent { get; set; }
    }

    public class RoomEvent<TRoomEventContent> : IRoomEvent
        where TRoomEventContent : class, IRoomContent
    {
        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }

        public static implicit operator RoomEvent<TRoomEventContent>(MessageRoomEvent messageEvent)
        {
            if (messageEvent == null || messageEvent.Content.GetType() != typeof(TRoomEventContent))
                return null;

            return new RoomEvent<TRoomEventContent>()
            {
                EventKind = messageEvent.EventKind,
                EventId = messageEvent.EventId,
                Sender = messageEvent.Sender,
                OriginServerTimestamp = messageEvent.OriginServerTimestamp,
                UnsignedData = messageEvent.UnsignedData,
                RoomId = messageEvent.RoomId,
                Content = (TRoomEventContent) messageEvent.Content
            };
        }

        public static implicit operator MessageRoomEvent(RoomEvent<TRoomEventContent> messageEvent)
        {
            //if (messageEvent == null) return null;

            return new MessageRoomEvent()
            {
                EventKind = messageEvent.EventKind,
                EventId = messageEvent.EventId,
                Sender = messageEvent.Sender,
                OriginServerTimestamp = messageEvent.OriginServerTimestamp,
                UnsignedData = messageEvent.UnsignedData,
                RoomId = messageEvent.RoomId,
                Content = (IMessageContent) messageEvent.Content
            };

            //new RoomEvent<TRoomEventContent>(messageEvent);
        }

        public TRoomEventContent Content { get; set; }

        IRoomContent IRoomEvent.Content
        {
            get => Content;
            set => Content = (TRoomEventContent)value;
        }

        public EventKind EventKind { get; set; }
    }

    public class StateEvent<TStateEventContent> : IStateEvent
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

        IStateContent? IStateEvent.PrevContent
        {
            get => PrevContent;
            set => PrevContent = (TStateEventContent) value;
        }

        IRoomContent IRoomEvent.Content
        {
            get => Content;
            set => Content = (TStateEventContent) value;
        }

        public EventKind EventKind { get; set; }
    }
    
    public sealed class PresenceEvent : IEvent
    {
        [JsonProperty(@"sender")] public string Sender { get; set; }

        [JsonProperty(@"content")] public PresenceContent Content { get; set; }
        public EventKind EventKind { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (PresenceContent) value;
        }

        public static string ToJsonString()
        {
            return @"m.presence";
        }
    }

    public sealed class TagEvent : IEvent
    {
        [JsonProperty(@"content")] public TagContent Content { get; set; }
        public EventKind EventKind { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (TagContent) value;
        }

        public static string ToJsonString()
        {
            return @"m.tag";
        }
    }

    public sealed class TypingEvent : IEvent
    {
        public string RoomId { get; set; }

        [JsonProperty(@"content")] public TypingContent Content { get; set; }
        public EventKind EventKind { get; set; }

        IEventContent IEvent.Content
        {
            get => Content;
            set => Content = (TypingContent) value;
        }

        public static string ToJsonString()
        {
            return @"m.typing";
        }
    }

    public sealed class FallbackEvent : IEvent
    {
        [JsonProperty(@"content")] 
        public IDictionary<string, string> Content { get; set; }

        [JsonProperty(@"type")]
        public string EventName { get; set; }

        EventKind IEvent.EventKind { get; set; } = EventKind.Custom;
        IEventContent IEvent.Content { get; set; }
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

    public sealed class RedactionRoomEvent : IRoomEvent
    {
        [JsonProperty(@"redacts")]
        public string RedactedEventId { get; set; }

        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }

        [JsonProperty(@"content")]
        public RedactionContent Content { get; set; }

        IRoomContent IRoomEvent.Content
        {
            get => Content;
            set => Content = (RedactionContent) value;
        }

        public EventKind EventKind { get; set; }
    }

    public sealed class MessageRoomEvent : IRoomEvent
    {
        public string EventId { get; set; }
        public string Sender { get; set; }
        public long OriginServerTimestamp { get; set; }
        public UnsignedData UnsignedData { get; set; }
        public string RoomId { get; set; }

        [JsonProperty(@"content")]
        public IMessageContent Content { get; set; }

        IRoomContent IRoomEvent.Content
        {
            get => Content;
            set => Content = (IMessageContent) value;
        }

        public EventKind EventKind { get; set; }
    }
}
