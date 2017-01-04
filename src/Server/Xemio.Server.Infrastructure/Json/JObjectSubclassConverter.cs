using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xemio.Server.Infrastructure.Json
{
    public class JObjectSubClassConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jObject = (JObject) value;
            jObject.WriteTo(writer, serializer.Converters.ToArray());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jobject = JObject.Load(reader);

            var jobjectSubclass = (JObject)Activator.CreateInstance(objectType); //Expect default constructor
            jobjectSubclass.Merge(jobject);

            return jobjectSubclass;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(JObject).IsAssignableFrom(objectType);
        }
    }
}