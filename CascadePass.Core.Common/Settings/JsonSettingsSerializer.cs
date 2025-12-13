using System;
using System.Text.Json;

namespace CascadePass.Core.Common.Settings
{
    public class JsonSettingsSerializer : ISettingsSerializer
    {
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
        };

        public string Serialize<T>(T settings)
        {
            return JsonSerializer.Serialize(settings, settings.GetType(), options);
        }

        public T Deserialize<T>(string data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(data);
            return JsonSerializer.Deserialize<T>(data, options)!;
        }
    }
}
