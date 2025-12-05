using System;
using System.Text.Json;

namespace CascadePass.Core.Common.Settings
{
    public class JsonSettingsSerializer : ISettingsSerializer
    {
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public string Serialize<T>(T settings)
        {
            return JsonSerializer.Serialize(settings, options);
        }

        public T Deserialize<T>(string data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(data);
            return JsonSerializer.Deserialize<T>(data, options)!;
        }
    }
}
