using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CascadePass.CascadeCore.UI
{
    public abstract class ThemeDetector : IThemeDetector, IDisposable
    {
        private readonly IRegistryProvider registryProvider;

        public event EventHandler ThemeChanged;

        #region Constructors

        public ThemeDetector()
        {
            this.registryProvider = new RegistryProvider();
            SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
            this.FollowSystemTheme = true;
        }

        public ThemeDetector(IRegistryProvider registryProviderToUse)
        {
            this.registryProvider = registryProviderToUse;
            SystemEvents.UserPreferenceChanged += this.OnUserPreferenceChanged;
            this.FollowSystemTheme = true;
        }

        #endregion

        #region Properties

        public bool FollowSystemTheme { get; set; }

        public IRegistryProvider RegistryProvider => this.registryProvider;

        public bool IsInLightMode =>
                    this.RegistryProvider.GetValue(
                        @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                        "AppsUseLightTheme"
                        )
                    ?.Equals(1) ?? true;

        public bool IsHighContrastEnabled => SystemParameters.HighContrast;

        #endregion

        #region Methods

        public void Dispose()
        {
            SystemEvents.UserPreferenceChanged -= this.OnUserPreferenceChanged;
        }

        public ThemeType GetThemeType()
        {
            if (this.IsHighContrastEnabled)
            {
                return ThemeType.HighContrast;
            }

            if (this.IsInLightMode)
            {
                return ThemeType.Light;
            }

            return ThemeType.Dark;
        }

        public abstract bool ApplyTheme();

        private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                this.OnThemeChanged(sender, e);
            }
        }

        protected async void OnThemeChanged(object sender, EventArgs e)
        {
            if (this.FollowSystemTheme)
            {
                await Task.Delay(50);
                this.ApplyTheme();
                this.ThemeChanged?.Invoke(this, e);
            }
        }

        #endregion
    }
}
