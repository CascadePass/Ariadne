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
            string directory = Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(directory))
            {
                // C:\Test\Company\Region\Product\Feature\File.txt
                // This call will create C:\Test if it's missing,
                // and so on for any folder in the path.
                //
                // There is no need to loop throuh the folder
                // levels and create each of them individually.

                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, this.Serialize());
        }

        public static T LoadFromFile<T>(string path, DefaultSettingsSerializationFormat format)
            where T : SettingsDocument
        {
            ISettingsSerializer serializer = format switch
            {
                DefaultSettingsSerializationFormat.Json => new JsonSettingsSerializer(),
                DefaultSettingsSerializationFormat.Xml => new XmlSettingsSerializer(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(format), format, null)
            };
            return SettingsDocument.LoadFromFile<T>(path, serializer);
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

        public abstract void Clamp();
    }
}
