using System;
using System.IO;

namespace CascadePass.Core.Common.Settings
{
    public abstract class SettingsDocument : Observable
    {
        #region Constructors

        protected SettingsDocument(ISettingsSerializer serializer)
        {
            this.Serializer = serializer;
        }

        protected SettingsDocument(DefaultSettingsSerializationFormat format)
        {
            this.Serializer = format switch
            {
                DefaultSettingsSerializationFormat.Json => new JsonSettingsSerializer(),
                DefaultSettingsSerializationFormat.Xml => new XmlSettingsSerializer(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }

        protected SettingsDocument()
        {
            this.Serializer = new JsonSettingsSerializer();
        }

        #endregion

        protected ISettingsSerializer Serializer { get; }

        public string Serialize()
        {
            return Serializer.Serialize(this);
        }

        public static T Deserialize<T>(string data, ISettingsSerializer serializer)
            where T : SettingsDocument
        {
            return serializer.Deserialize<T>(data);
        }

        public virtual void SaveToFile(string path)
        {
            File.WriteAllText(path, this.Serialize());
        }

        public static T LoadFromFile<T>(string path, ISettingsSerializer serializer)
            where T : SettingsDocument
        {
            if (!File.Exists(path))
                return Activator.CreateInstance<T>();

            var data = File.ReadAllText(path);
            return SettingsDocument.Deserialize<T>(data, serializer);
        }

        public abstract void ResetToDefaults();
    }
}
