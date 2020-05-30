using System;

using Matrix.Api.ClientServer.Enumerations;

using Newtonsoft.Json;

namespace Matrix.Json
{
    internal class ErrorCodeJsonConverter : JsonConverter<ErrorCode>
    {
        public override void WriteJson(JsonWriter writer, ErrorCode value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override ErrorCode ReadJson(JsonReader reader, Type objectType, ErrorCode existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var errorCode = reader.ReadAsString();
            if (errorCode == null) throw new NullReferenceException(@"Failed to read errcode property.");
            return errorCode.ToErrorCode();
        }
    }

    internal class AuthenticationKindJsonConverter : JsonConverter<AuthenticationKind>
    {
        public override void WriteJson(JsonWriter writer, AuthenticationKind value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToJsonString());
        }

        public override AuthenticationKind ReadJson(JsonReader reader, Type objectType, AuthenticationKind existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));
            var authenticationKindString = reader.ReadAsString();
            if (authenticationKindString == null) throw new NullReferenceException(@"Failed to read authentication type property.");
            return authenticationKindString.ToAuthenticationKind();
        }
    }
}