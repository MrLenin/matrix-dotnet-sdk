using System;
using System.Collections.Generic;
using Matrix.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public class JsonEventConverter : JsonConverter
    {
        private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer;
        private Newtonsoft.Json.JsonSerializer _tempSerializer;

        private Dictionary<string, Type> contentTypes = new Dictionary<string, Type>
        {
            {"m.presence", typeof(MatrixMPresence)},
            {"m.receipt", typeof(MatrixMReceipt)}, //*Special case below
            {"m.room.message", typeof(MatrixMRoomMessage)},
            {"m.room.member", typeof(MatrixMRoomMember)},
            {"m.room.create", typeof(MatrixMRoomCreate)},
            {"m.room.join_rules", typeof(MatrixMRoomJoinRules)},
            {"m.room.aliases", typeof(MatrixMRoomAliases)},
            {"m.room.canonical_alias", typeof(MatrixMRoomCanonicalAlias)},
            {"m.room.name", typeof(MatrixMRoomName)},
            {"m.room.topic", typeof(MatrixMRoomTopic)},
            {"m.room.power_levels", typeof(MatrixMRoomPowerLevels)},
            {"m.room.history_visibility", typeof(MatrixMRoomHistoryVisibility)},
            {"m.typing", typeof(MatrixMTyping)}
        };

        private Dictionary<string, Type> messageContentTypes = new Dictionary<string, Type>
        {
            {"m.text", typeof(MMessageText)},
            {"m.notice", typeof(MMessageNotice)},
            {"m.emote", typeof(MMessageEmote)},
            {"m.image", typeof(MMessageImage)},
            {"m.file", typeof(MMessageFile)},
            {"m.location", typeof(MMessageLocation)}
        };

        public JsonEventConverter(Dictionary<string, Type> customMsgTypes = null)
        {
            _jsonSerializer = new Newtonsoft.Json.JsonSerializer();

            if (customMsgTypes == null) return;

            foreach (var (messageId, type) in customMsgTypes)
                if (contentTypes.ContainsKey(messageId))
                    contentTypes[messageId] = type;
                else
                    contentTypes.Add(messageId, type);
        }

        public void AddMessageType(string messageId, Type type)
        {
            messageContentTypes.Add(messageId, type);
        }

        public void AddEventType(string messageId, Type type)
        {
            contentTypes.Add(messageId, type);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MatrixEvent) || objectType == typeof(MatrixEventContent);
        }

        public Type MessageContentType(string type)
        {
            return messageContentTypes.TryGetValue(type, out var outType) ? outType : typeof(MatrixMRoomMessage);
        }

        public MatrixEventContent GetContent(JObject jObject, string type)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            if (!contentTypes.TryGetValue(type, out var T))
                return new MatrixEventContent {MxContent = jObject};

            _tempSerializer ??= _jsonSerializer;

            try
            {
                if (T == typeof(MatrixMRoomMessage))
                {
                    var message = new MatrixMRoomMessage();
                    _tempSerializer.Populate(jObject.CreateReader(), message);
                    T = MessageContentType(message.MessageType);
                }

                var content = (MatrixEventContent) Activator.CreateInstance(T);
                content.MxContent = jObject;

                if (type == "m.receipt")
                    ((MatrixMReceipt) content).ParseJObject(jObject);
                else
                    _tempSerializer.Populate(jObject.CreateReader(), content);

                return content;
            }
            catch (Exception)
            {
                throw new Exception($"Failed to get content for {type}");
            }
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _tempSerializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

            // Load JObject from stream
            var jObject = JObject.Load(reader);

            // Populate MatrixEventContent if applicable
            if (objectType != typeof(MatrixEvent))
                return objectType != typeof(MatrixEventContent) ? null : GetContent(jObject, "");

            // Populate the event itself
            var ev = new MatrixEvent();

            serializer.Populate(jObject.CreateReader(), ev);

            if (jObject["content"].HasValues)
            {
                ev.Content = GetContent(jObject["content"] as JObject, ev.Type);
            }
            else if (((JObject) jObject["unsigned"]).TryGetValue("redacted_because", out _))
            {
                //TODO: Parse Redacted
            }

            return ev;
        }

        public override bool CanWrite => false;

        public override void WriteJson(
            JsonWriter writer,
            object value,
            Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}