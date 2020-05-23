using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Matrix
{
	public class JsonSerializer : Newtonsoft.Json.JsonSerializer
	{
		public JsonSerializer ()
		{
			NullValueHandling = NullValueHandling.Ignore;
			Converters.Add(new JsonEnumConverter());
			Converters.Add(new JSONEventConverter());
		}
	}
	public class JsonEnumConverter : JsonConverter
    {
		public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
		{
			if (value == null) throw new ArgumentNullException(nameof(value));

			var t = value.GetType ();
			var name = Enum.GetName(t, value)?.ToLower();

			JToken.FromObject(name).WriteTo(writer);
		}

		public override bool CanConvert(Type objectType)
		{
			if (objectType == null) throw new ArgumentNullException(nameof(objectType));
            return objectType.IsEnum;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}
	}
}
