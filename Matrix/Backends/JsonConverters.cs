using System;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.Versions;

using Newtonsoft.Json;

namespace Matrix.Backends
{
    namespace JsonConverters
    {
        public class AuthenticationKindJsonConverter : JsonConverter<AuthenticationKind>
        {
            public override void WriteJson(JsonWriter writer, AuthenticationKind value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override AuthenticationKind ReadJson(JsonReader reader, Type objectType,
                AuthenticationKind existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read authentication type property.");
                return stringValue.ToAuthenticationKind();
            }
        }

        public class ErrorCodeJsonConverter : JsonConverter<ErrorCode>
        {
            public override void WriteJson(JsonWriter writer, ErrorCode value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override ErrorCode ReadJson(JsonReader reader, Type objectType, ErrorCode existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read errcode property.");
                return stringValue.ToErrorCode();
            }
        }

        public class EventKindJsonConverter : JsonConverter<EventKind>
        {
            public override void WriteJson(JsonWriter writer, EventKind value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override EventKind ReadJson(JsonReader reader, Type objectType, EventKind existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read event type property.");
                return stringValue.ToEventKind();
            }
        }

        public class GuestAccessKindJsonConverter : JsonConverter<GuestAccess>
        {
            public override void WriteJson(JsonWriter writer, GuestAccess value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override GuestAccess ReadJson(JsonReader reader, Type objectType, GuestAccess existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read guest_access property.");
                return stringValue.ToGuestAccess();
            }
        }

        public class HistoryVisibilityKindJsonConverter : JsonConverter<HistoryVisibility>
        {
            public override void WriteJson(JsonWriter writer, HistoryVisibility value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override HistoryVisibility ReadJson(JsonReader reader, Type objectType,
                HistoryVisibility existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read history_visibility property.");
                return stringValue.ToHistoryVisibility();
            }
        }

        public class JoinRuleKindJsonConverter : JsonConverter<JoinRule>
        {
            public override void WriteJson(JsonWriter writer, JoinRule value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override JoinRule ReadJson(JsonReader reader, Type objectType, JoinRule existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read join_rule property.");
                return stringValue.ToJoinRule();
            }
        }

        public class MembershipStateJsonConverter : JsonConverter<MembershipState>
        {
            public override void WriteJson(JsonWriter writer, MembershipState value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override MembershipState ReadJson(JsonReader reader, Type objectType, MembershipState existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read membership property.");
                return stringValue.ToMembershipState();
            }
        }

        public class MessageKindJsonConverter : JsonConverter<MessageKind>
        {
            public override void WriteJson(JsonWriter writer, MessageKind value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override MessageKind ReadJson(JsonReader reader, Type objectType, MessageKind existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read message type property.");
                return stringValue.ToMessageKind();
            }
        }

        public class PresenceStatusJsonConverter : JsonConverter<PresenceState>
        {
            public override void WriteJson(JsonWriter writer, PresenceState value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override PresenceState ReadJson(JsonReader reader, Type objectType, PresenceState existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read presence property.");
                return stringValue.ToPresenceState();
            }
        }

        public class ClientServerVersionJsonConverter : JsonConverter<ClientServerVersion>
        {
            public override void WriteJson(JsonWriter writer, ClientServerVersion value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override ClientServerVersion ReadJson(JsonReader reader, Type objectType,
                ClientServerVersion existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read client-server version property.");
                return stringValue.ToClientServerVersion();
            }
        }

        public class RoomsVersionsJsonConverter : JsonConverter<RoomsVersion>
        {
            public override void WriteJson(JsonWriter writer, RoomsVersion value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToJsonString());
            }

            public override RoomsVersion ReadJson(JsonReader reader, Type objectType, RoomsVersion existingValue,
                bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (reader == null) throw new ArgumentNullException(nameof(reader));
                if (!(reader.Value is string stringValue))
                    throw new NullReferenceException(@"Failed to read room version property.");
                return stringValue.ToRoomsVersion();
            }
        }
    }
}