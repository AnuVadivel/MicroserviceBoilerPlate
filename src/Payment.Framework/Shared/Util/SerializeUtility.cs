using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Payment.Framework.Shared.Extension;

namespace Payment.Framework.Shared.Util
{
    public static class SerializeUtility
    {
        private static readonly JsonSerializerOptions SerializeOptions = new()
        {
            AllowTrailingCommas = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private static readonly JsonSerializerOptions DeserializeOptions = new()
        {
            AllowTrailingCommas = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            PropertyNameCaseInsensitive = true,
        };

        public static string Serialize(object input) =>
            JsonSerializer.Serialize(input, SerializeOptions);

        public static T Deserialize<T>(string input) =>
            input.IsNullOrWhiteSpace() ? default : JsonSerializer.Deserialize<T>(input, DeserializeOptions);

        public static T Deserialize<T>(byte[] input) =>
            JsonSerializer.Deserialize<T>(input, DeserializeOptions);

        public static T Deserialize<T>(Stream input) =>
            input.HasValue()
                ? JsonSerializer.Deserialize<T>(input, DeserializeOptions)
                : default;
    }
}