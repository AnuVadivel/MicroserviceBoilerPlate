using System.IO;
using Newtonsoft.Json;
using Payment.Framework.Shared.Extension;

namespace Payment.Framework.Shared.Util
{
    public static class NewtonSoftSerializeUtility
    {
        public static string Serialize(object input) => 
            JsonConvert.SerializeObject(input);

        public static T Deserialize<T>(string input) => 
            input.IsNullOrWhiteSpace() ? default : JsonConvert.DeserializeObject<T>(input);

        public static T Deserialize<T>(Stream input)
        {
            if (!input.HasValue()) return default;

            var serializer = new JsonSerializer();
            using var streamReader = new StreamReader(input);
            var data = (T)serializer.Deserialize(streamReader, typeof(T));
            return data;
        }
    }
}