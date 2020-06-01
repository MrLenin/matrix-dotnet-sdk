using System;

using Matrix.Api.ClientServer.Enumerations;
using Matrix.Api.Versions;

using Newtonsoft.Json;

namespace Matrix.Json
{
    public class AuthenticationKindJsonConverter : JsonConverter<AuthenticationKind>
    {
        public override void WriteJson(JsonWriter writer, AuthenticationKind value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override AuthenticationKind ReadJson(JsonReader reader, Type objectType, AuthenticationKind existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read authentication type property.");
            return stringValue.ToAuthenticationKind();
        }
    }

    public class ErrorCodeJsonConverter : JsonConverter<ErrorCode>
    {
        public override void WriteJson(JsonWriter writer, ErrorCode value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override ErrorCode ReadJson(JsonReader reader, Type objectType, ErrorCode existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read errcode property.");
            return stringValue.ToErrorCode();
        }
    }

    public class EventKindJsonConverter : JsonConverter<EventKind>
    {
        public override void WriteJson(JsonWriter writer, EventKind value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }
        
        public override EventKind ReadJson(JsonReader reader, Type objectType, EventKind existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read event type property.");
            return stringValue.ToEventKind();
        }
    }

    public class GuestAccessKindJsonConverter : JsonConverter<GuestAccessKind>
    {
        public override void WriteJson(JsonWriter writer, GuestAccessKind value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override GuestAccessKind ReadJson(JsonReader reader, Type objectType, GuestAccessKind existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read guest_access property.");
            return stringValue.ToGuestAccessKind();
        }
    }

    public class HistoryVisibilityKindJsonConverter : JsonConverter<HistoryVisibilityKind>
    {
        public override void WriteJson(JsonWriter writer, HistoryVisibilityKind value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override HistoryVisibilityKind ReadJson(JsonReader reader, Type objectType, HistoryVisibilityKind existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read history_visibility property.");
            return stringValue.ToHistoryVisibilityKind();
        }
    }

    public class JoinRuleKindJsonConverter : JsonConverter<JoinRule>
    {
        public override void WriteJson(JsonWriter writer, JoinRule value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override JoinRule ReadJson(JsonReader reader, Type objectType, JoinRule existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read join_rule property.");
            return stringValue.ToJoinRuleKind();
        }
    }

    public class MembershipStateJsonConverter : JsonConverter<MembershipState>
    {
        public override void WriteJson(JsonWriter writer, MembershipState value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override MembershipState ReadJson(JsonReader reader, Type objectType, MembershipState existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read membership property.");
            return stringValue.ToMembershipState();
        }
    }

    public class MessageKindJsonConverter : JsonConverter<MessageKind>
    {
        public override void WriteJson(JsonWriter writer, MessageKind value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override MessageKind ReadJson(JsonReader reader, Type objectType, MessageKind existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read message type property.");
            return stringValue.ToMessageKind();
        }
    }

    public class PresenceStatusJsonConverter : JsonConverter<PresenceStatus>
    {
        public override void WriteJson(JsonWriter writer, PresenceStatus value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override PresenceStatus ReadJson(JsonReader reader, Type objectType, PresenceStatus existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read presence property.");
            return stringValue.ToPresenceStatus();
        }
    }

    public class ClientServerVersionJsonConverter : JsonConverter<ClientServerVersion>
    {
        public override void WriteJson(JsonWriter writer, ClientServerVersion value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override ClientServerVersion ReadJson(JsonReader reader, Type objectType, ClientServerVersion existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read client-server version property.");
            return stringValue.ToClientServerVersion();
        }
    }

    public class RoomsVersionsJsonConverter : JsonConverter<RoomsVersion>
    {
        public override void WriteJson(JsonWriter writer, RoomsVersion value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override RoomsVersion ReadJson(JsonReader reader, Type objectType, RoomsVersion existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            if (!(reader.Value is string stringValue)) throw new NullReferenceException(@"Failed to read room version property.");
            return stringValue.ToRoomsVersion();
        }
    }
}