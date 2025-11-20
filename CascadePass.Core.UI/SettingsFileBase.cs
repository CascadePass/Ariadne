using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CascadePass.Core.UI
{
    public abstract class SettingsDocumentBase
    {
    }

    public enum SerializationFormat
    {
        None,
        Json,
        Xml,
        Yaml,
        Ini
    }

    public interface IWindowSizeAndPosition
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }
    }

    //public abstract class DocumentSerializer
    //{
    //    public abstract string Serialize<T>(T document);

    //    public abstract static T Deserialize<T>(string content, SerializationFormat format)
    //    {
    //    }
    //}

    //public class JsonDocumentSerializer : DocumentSerializer
    //{
    //}
}
