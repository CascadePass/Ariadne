using CascadePass.Core.Common.Settings;
using System;

namespace CascadePass.Core.Common.Tests.Settings
{
    [TestClass]
    public class JsonSettingsSerializerTests
    {
        [TestMethod]
        public void Deserialize_InvalidJson_ThrowsException()
        {
            var serializer = new JsonSettingsSerializer();
            var badjson = "{\r\n  \"Theme\": \"Light\",\r\n  \"TitleBarMode\""; // missing closing curly bracket

            Assert.Throws<NotSupportedException>(() =>
                serializer.Deserialize<SettingsDocument>(badjson));
        }

        [TestMethod]
        public void SerializeAndDeserialize_RoundTrip_PreservesValues()
        {
            var serializer = new JsonSettingsSerializer();
            var settings = new ExampleJsonSettings { Theme = "Dark", TitleBarMode = TitleBarContentMode.FileName };

            var json = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleJsonSettings>(json);

            Assert.AreEqual(settings.Theme, result.Theme);
            Assert.AreEqual(settings.TitleBarMode, result.TitleBarMode);
        }

        [TestMethod]
        public void Serialize_EmptyObject_ProducesValidXml()
        {
            var serializer = new JsonSettingsSerializer();
            var settings = new ExampleJsonSettings();

            var json = serializer.Serialize(settings);
            Assert.IsTrue(json.Contains(nameof(ExampleJsonSettings.TitleBarMode)));
            Assert.IsTrue(json.Contains(nameof(ExampleJsonSettings.Theme)));

            var result = serializer.Deserialize<ExampleJsonSettings>(json);
            Assert.AreEqual("Light", result.Theme); // Default
        }

        [TestMethod]
        public void Serialize_SpecialCharacters_RoundTripPreservesValue()
        {
            var serializer = new JsonSettingsSerializer();
            var settings = new ExampleJsonSettings { Theme = "Dark & Light: <Test>" };

            var json = serializer.Serialize(settings);

            var result = serializer.Deserialize<ExampleJsonSettings>(json);
            Assert.AreEqual(settings.Theme, result.Theme);
        }

        [TestMethod]
        public void Serialize_MultipleProperties_RoundTripPreservesAllValues()
        {
            var serializer = new JsonSettingsSerializer();
            var settings = new ExampleJsonSettings
            {
                Theme = "Dark",
                TitleBarMode = TitleBarContentMode.SearchBar
            };

            var json = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleJsonSettings>(json);

            Assert.AreEqual("Dark", result.Theme);
            Assert.AreEqual(TitleBarContentMode.SearchBar, result.TitleBarMode);
        }

        [TestMethod]
        public void SerializeAndDeserialize_DefaultValues_PreservesDefaults()
        {
            var serializer = new JsonSettingsSerializer();
            var settings = new ExampleJsonSettings(); // defaults: Theme=Light, TitleBarMode=SearchBar

            var json = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleJsonSettings>(json);

            Assert.AreEqual("Light", result.Theme);
            Assert.AreEqual(TitleBarContentMode.SearchBar, result.TitleBarMode);
        }

        [TestMethod]
        public void Deserialize_EmptyString_ThrowsArgumentException()
        {
            var serializer = new JsonSettingsSerializer();
            Assert.Throws<ArgumentException>(() =>
                serializer.Deserialize<ExampleJsonSettings>(string.Empty));
        }

        [TestMethod]
        public void SerializeAndDeserialize_MultipleProperties_PreservesAllValues()
        {
            var serializer = new JsonSettingsSerializer();
            var settings = new ExampleJsonSettings
            {
                Theme = "Custom",
                TitleBarMode = TitleBarContentMode.FileName
            };

            var json = serializer.Serialize(settings);
            var result = serializer.Deserialize<ExampleJsonSettings>(json);

            Assert.AreEqual("Custom", result.Theme);
            Assert.AreEqual(TitleBarContentMode.FileName, result.TitleBarMode);
        }
    }
}
