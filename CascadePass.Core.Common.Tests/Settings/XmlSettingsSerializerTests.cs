using CascadePass.Core.Common.Settings;
using System;

namespace CascadePass.Core.Common.Tests.Settings
{
    [TestClass]
    public class XmlSettingsSerializerTests
    {
        [TestMethod]
        public void Deserialize_InvalidXml_ThrowsException()
        {
            var serializer = new XmlSettingsSerializer();
            var badXml = "<AppSettings><Theme>Dark"; // missing closing tags

            Assert.Throws<InvalidOperationException>(() =>
                serializer.Deserialize<SettingsDocument>(badXml));
        }

        [TestMethod]
        public void SerializeAndDeserialize_RoundTrip_PreservesValues()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings { Theme = "Dark", TitleBarMode = TitleBarContentMode.FileName };

            var xml = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleXmlSettings>(xml);

            Assert.AreEqual(settings.Theme, result.Theme);
            Assert.AreEqual(settings.TitleBarMode, result.TitleBarMode);
        }

        [TestMethod]
        public void Serialize_EmptyObject_ProducesValidXml()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings();

            var xml = serializer.Serialize(settings);
            Assert.IsTrue(xml.Contains(nameof(ExampleXmlSettings))); // Root element present

            var result = serializer.Deserialize<ExampleXmlSettings>(xml);
            Assert.AreEqual("Light", result.Theme); // Default
        }

        [TestMethod]
        public void Serialize_SpecialCharacters_RoundTripPreservesValue()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings { Theme = "Dark & Light <Test>" };

            var xml = serializer.Serialize(settings);
            Assert.IsTrue(xml.Contains("&amp;")); // encoded

            var result = serializer.Deserialize<ExampleXmlSettings>(xml);
            Assert.AreEqual(settings.Theme, result.Theme);
        }

        [TestMethod]
        public void Serialize_MultipleProperties_RoundTripPreservesAllValues()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings
            {
                Theme = "Dark",
                TitleBarMode = TitleBarContentMode.SearchBar
            };

            var xml = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleXmlSettings>(xml);

            Assert.AreEqual("Dark", result.Theme);
            Assert.AreEqual(TitleBarContentMode.SearchBar, result.TitleBarMode);
        }

        [TestMethod]
        public void SerializeAndDeserialize_DefaultValues_PreservesDefaults()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings(); // defaults: Theme=Light, TitleBarMode=SearchBar

            var xml = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleXmlSettings>(xml);

            Assert.AreEqual("Light", result.Theme);
            Assert.AreEqual(TitleBarContentMode.SearchBar, result.TitleBarMode);
        }

        [TestMethod]
        public void Serialize_SpecialCharacters_EncodesCorrectly()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings { Theme = "Dark & Light <Test>" };

            var xml = serializer.Serialize(settings);
            Assert.IsTrue(xml.Contains("&amp;")); // XML encoding check

            var result = serializer.Deserialize<ExampleXmlSettings>(xml);
            Assert.AreEqual(settings.Theme, result.Theme);
        }

        [TestMethod]
        public void Deserialize_EmptyString_ThrowsArgumentException()
        {
            var serializer = new XmlSettingsSerializer();
            Assert.Throws<ArgumentException>(() =>
                serializer.Deserialize<ExampleXmlSettings>(string.Empty));
        }

        [TestMethod]
        public void SerializeAndDeserialize_MultipleProperties_PreservesAllValues()
        {
            var serializer = new XmlSettingsSerializer();
            var settings = new ExampleXmlSettings
            {
                Theme = "Custom",
                TitleBarMode = TitleBarContentMode.FileName
            };

            var xml = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleXmlSettings>(xml);

            Assert.AreEqual("Custom", result.Theme);
            Assert.AreEqual(TitleBarContentMode.FileName, result.TitleBarMode);
        }
    }
}
