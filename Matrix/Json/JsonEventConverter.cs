//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.CodeAnalysis;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices.ComTypes;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//using Matrix.Api;
//using Matrix.Api.ClientServer;
//using Matrix.Structures;

//namespace Matrix.Json
//{



//    public class EventTypeConverter : JsonConverter<IEvent>
//    {
//        public EventTypeConverter()
//        {
//        }

//        public void AddMessageType(string messageId, Type type)
//        {
//            messageContentTypes.Add(messageId, type);
//        }

//        public void AddEventType(string messageId, Type type)
//        {
//            contentTypes.Add(messageId, type);
//        }

//        public override IEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
////             using var eventDocument = JsonDocument.ParseValue(ref reader);
////             var eventRoot = eventDocument.RootElement.Clone();
////             var contentElement = eventRoot.GetProperty(@"content");
////             var typeElement = eventRoot.GetProperty(@"type");

//            void validateToken(Utf8JsonReader reader, JsonTokenType tokenType)
//            {
//                if (reader.TokenType != tokenType)
//                    throw new JsonException(
//                        $"Invalid token: Was expecting a '{tokenType}' teken, but received a '{reader.TokenType}' token.");
//            }

//            validateToken(reader, JsonTokenType.StartObject);
//            reader.Read();
//            validateToken(reader, JsonTokenType.PropertyName);
//            var obj = reader.ValueTextEquals()

//            // Populate MatrixEventContent if applicable
//            if (typeToConvert != typeof(IEvent))
//                return typeToConvert != typeof(MatrixEventContent) ? null : GetContent(jsonDocument, "");

//            // Populate the event itself
//            var ev = new MatrixEvent();

//            serializer.Populate(jsonDocument.CreateReader(), ev);

//            JsonElement jsonElement;

//            if (jsonDocument.RootElement.TryGetProperty("content", out jsonElement))
//            {
//                ev.Content = GetContent(jsonElement, ev.Type);
//            }
//            else if (((JsonDocument)jObject["unsigned"]).TryGetValue("redacted_because", out _))
//            {
//                //TODO: Parse Redacted
//            }

//            return ev;
//        }

//        public override void Write(Utf8JsonWriter writer, IEvent value, JsonSerializerOptions options)
//        {
//            throw new NotImplementedException();
//        }

//        public Type MessageContentType(string type)
//        {
//            return messageContentTypes.TryGetValue(type, out var outType) ? outType : typeof(MatrixMRoomMessage);
//        }

//        public MatrixEventContent GetContent(JsonDocument jObject, string type)
//        {
//            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

//            if (!contentTypes.TryGetValue(type, out var T))
//                return new MatrixEventContent { MxContent = jObject };

//            try
//            {
//                if (T == typeof(MatrixMRoomMessage))
//                {
//                    var message = new MatrixMRoomMessage();
//                    _tempSerializer.Populate(jObject.CreateReader(), message);
//                    T = MessageContentType(message.MessageType);
//                }

//                var content = (MatrixEventContent)Activator.CreateInstance(T);
//                content.MxContent = jObject;

//                if (type == "m.receipt")
//                    ((ReceiptContent)content).ParseJObject(jObject);
//                else
//                    _tempSerializer.Populate(jObject.CreateReader(), content);

//                return content;
//            }
//            catch (Exception)
//            {
//                throw new Exception($"Failed to get content for {type}");
//            }
//        }

//        public override object ReadJson(
//            JsonReader reader,
//            Type objectType,
//            object existingValue,
//            Newtonsoft.Json.JsonSerializer serializer)
//        {
//            if (reader == null) throw new ArgumentNullException(nameof(reader));

//            _tempSerializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

//            // Load JObject from stream
//            var jObject = JsonDocument.Load(reader);

//            // Populate MatrixEventContent if applicable
//            if (objectType != typeof(MatrixEvent))
//                return objectType != typeof(MatrixEventContent) ? null : GetContent(jObject, "");

//            // Populate the event itself
//            var ev = new MatrixEvent();

//            serializer.Populate(jObject.CreateReader(), ev);

//            if (jObject["content"].HasValues)
//            {
//                ev.Content = GetContent(jObject["content"] as JsonDocument, ev.Type);
//            }
//            else if (((JsonDocument)jObject["unsigned"]).TryGetValue("redacted_because", out _))
//            {
//                //TODO: Parse Redacted
//            }

//            return ev;
//        }

//        public override bool CanWrite => false;

//        public override void WriteJson(
//            JsonWriter writer,
//            object value,
//            Newtonsoft.Json.JsonSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }
//        private Dictionary<string, Type> contentTypes = new Dictionary<string, Type>
//        {
//            {"m.presence", typeof(PresenceContent)},
//            {"m.receipt", typeof(ReceiptContent)}, //*Special case below
//            {"m.room.message", typeof(MatrixMRoomMessage)},
//            {"m.room.member", typeof(MatrixMRoomMember)},
//            {"m.room.create", typeof(MatrixMRoomCreate)},
//            {"m.room.join_rules", typeof(MatrixMRoomJoinRules)},
//            {"m.room.aliases", typeof(MatrixMRoomAliases)},
//            {"m.room.canonical_alias", typeof(MatrixMRoomCanonicalAlias)},
//            {"m.room.name", typeof(MatrixMRoomName)},
//            {"m.room.topic", typeof(MatrixMRoomTopic)},
//            {"m.room.power_levels", typeof(MatrixMRoomPowerLevels)},
//            {"m.room.history_visibility", typeof(MatrixMRoomHistoryVisibility)},
//            {"m.typing", typeof(MatrixMTyping)}
//        };

//        private Dictionary<string, Type> messageContentTypes = new Dictionary<string, Type>
//        {
//            {"m.text", typeof(MMessageText)},
//            {"m.notice", typeof(MMessageNotice)},
//            {"m.emote", typeof(MMessageEmote)},
//            {"m.image", typeof(MMessageImage)},
//            {"m.file", typeof(MMessageFile)},
//            {"m.location", typeof(MMessageLocation)}
//        };
//    }
//}