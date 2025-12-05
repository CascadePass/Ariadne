using CascadePass.Core.Common.Settings;

namespace CascadePass.Core.Common.Tests.Settings
{
    public class ExampleXmlSettings : SettingsDocument
    {
        public ExampleXmlSettings() : base(DefaultSettingsSerializationFormat.Xml) { }

        public string Theme { get; set; } = "Light";

        public TitleBarContentMode TitleBarMode { get; set; } = TitleBarContentMode.SearchBar;

        public override void ResetToDefaults()
        {
            Theme = "Light";
            TitleBarMode = TitleBarContentMode.SearchBar;
        }
    }

    public class ExampleJsonSettings : SettingsDocument
    {
        public ExampleJsonSettings() : base(DefaultSettingsSerializationFormat.Json) { }

        public string Theme { get; set; } = "Light";

        public TitleBarContentMode TitleBarMode { get; set; } = TitleBarContentMode.SearchBar;

        public override void ResetToDefaults()
        {
            Theme = "Light";
            TitleBarMode = TitleBarContentMode.SearchBar;
        }
    }

    public enum TitleBarContentMode
    {
        None,
        SearchBar,
        FileName
    }
}
