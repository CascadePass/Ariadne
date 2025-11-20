using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CascadePass.Core.UI.Tests
{
    [TestClass]
    public class ThemeDetectorTests
    {
        [TestMethod]
        public void GetThemeType_LightMode_ReturnsLight()
        {
            var registryStub = new StubRegistryProvider(1); // Simulate light mode
            var detector = new TestThemeDetector(registryStub, false); // Simulate normal contrast

            var result = detector.GetThemeType();

            Assert.AreEqual(ThemeType.Light, result);
        }

        [TestMethod]
        public void GetThemeType_HighContrastEnabled_ReturnsDark()
        {
            var registryStub = new StubRegistryProvider(0);
            var detector = new TestThemeDetector(registryStub, true);

            var result = detector.GetThemeType();

            Assert.AreEqual(ThemeType.Dark, result);
        }

        public class StubRegistryProvider : IRegistryProvider
        {
            private readonly int? value;
            public StubRegistryProvider(int? appsUseLightTheme) => this.value = appsUseLightTheme;

            public event EventHandler<RegistryAccessEventArgs> RegistryAccessed;

            public bool DeleteValue(string keyName, string valueName)
            {
                throw new NotImplementedException();
            }

            public string[] GetSubKeyNames(string keyName)
            {
                throw new NotImplementedException();
            }

            public object GetValue(string key, string name) => this.value;

            public string[] GetValueNames(string keyName)
            {
                throw new NotImplementedException();
            }

            public bool SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
            {
                throw new NotImplementedException();
            }
        }


        public class TestThemeDetector : ThemeDetector
        {
            private readonly bool highContrast;
            public bool ApplyThemeCalled { get; private set; }

            public TestThemeDetector(IRegistryProvider provider, bool highContrastEnabled)
                : base(provider)
            {
                this.highContrast = highContrastEnabled;
            }

            public override bool ApplyTheme()
            {
                this.ApplyThemeCalled = true;
                return true;
            }

            public bool IsHighContrastEnabled => this.highContrast;
        }

    }
}
