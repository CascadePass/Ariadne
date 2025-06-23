using System;

namespace CascadePass.CascadeCore.UI
{
    public interface IThemeDetector
    {
        bool FollowSystemTheme { get; set; }
        bool IsHighContrastEnabled { get; }
        bool IsInLightMode { get; }
        IRegistryProvider RegistryProvider { get; }

        event EventHandler ThemeChanged;

        bool ApplyTheme();
        void Dispose();
        ThemeType GetThemeType();
    }
}