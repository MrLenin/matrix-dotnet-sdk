using System;
using System.Collections.Generic;
using Matrix.Structures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix
{
    public class JSONEventConverter : JsonConverter
    {
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

        public JSONEventConverter(Dictionary<string, Type> customMsgTypes = null)
        {
            if (customMsgTypes != null)
                foreach (var item in customMsgTypes)
                    if (contentTypes.ContainsKey(item.Key))
                        contentTypes[item.Key] = item.Value;
                    else
                        contentTypes.Add(item.Key, item.Value);
        }

        public void AddMessageType(string name, Type type)
        {
            messageContentTypes.Add(name, type);
        }

        public void AddEventType(string msgtype, Type type)
        {
            contentTypes.Add(msgtype, type);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MatrixEvent) || objectType == typeof(MatrixEventContent);
        }

        public Type MessageContentType(string type)
        {
            Type otype;
            if (messageContentTypes.TryGetValue(type, out otype)) return otype;

            return typeof(MatrixMRoomMessage);
        }

        public MatrixEventContent GetContent(JObject jObject, Newtonsoft.Json.JsonSerializer serializer, string type)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            if (!contentTypes.TryGetValue(type, out var T))
                return new MatrixEventContent {mxContent = jObject};

            try
            {
                if (T == typeof(MatrixMRoomMessage))
                {
                    var message = new MatrixMRoomMessage();
                    serializer.Populate(jObject.CreateReader(), message);
                    T = MessageContentType(message.msgtype);
                }

                var content = (MatrixEventContent) Activator.CreateInstance(T);
                content.mxContent = jObject;

                if (type == "m.receipt")
                    ((MatrixMReceipt) content).ParseJObject(jObject);
                else
                    serializer.Populate(jObject.CreateReader(), content);

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
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            // Load JObject from stream
            var jObject = JObject.Load(reader);

            // Populate MatrixEventContent if applicable
            if (objectType != typeof(MatrixEvent))
                return objectType != typeof(MatrixEventContent) ? null : GetContent(jObject, serializer, "");

            // Populate the event itself
            var ev = new MatrixEvent();

            serializer.Populate(jObject.CreateReader(), ev);

            if (jObject["content"].HasValues)
            {
                ev.content = GetContent(jObject["content"] as JObject, serializer, ev.type);
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