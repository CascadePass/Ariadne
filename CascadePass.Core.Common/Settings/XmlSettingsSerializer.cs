using System;
using System.IO;
using System.Xml.Serialization;

namespace CascadePass.Core.Common.Settings
{
    public class XmlSettingsSerializer : ISettingsSerializer
    {
        public string Serialize<T>(T settings)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StringWriter();
            serializer.Serialize(writer, settings);
            return writer.ToString();
        }

        public T Deserialize<T>(string data)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(data);

            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(data);
            return (T)serializer.Deserialize(reader)!;
        }
    }
}
