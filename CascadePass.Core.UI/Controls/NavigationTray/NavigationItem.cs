using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CascadePass.Core.UI.Controls
{
    public class NavigationItem : Observable
    {
        #region Private fields

        private string textLabel;
        private string iconUri, selectedIconUri;
        private bool isSelected, showSelectedIcon, isVisible;
        private object tooltip, tag, commandParameter;
        private ICommand command;

        private FontWeight fontWeight;
        private Brush foreground, background, mouseOver, pressed;
        private CornerRadius cornerRadius;

        #endregion

        public NavigationItem()
        {
            this.isVisible = true;
            this.fontWeight = FontWeights.Normal;
        }

        public string Label
        {
            get => this.textLabel;
            set => this.SetPropertyValue(ref this.textLabel, value, nameof(this.Label));
        }

        public string Icon {
            get => this.iconUri;
            set => this.SetPropertyValue(ref this.iconUri, value, [nameof(this.Icon), nameof(this.EffectiveSelectedIcon), nameof(this.IconPath),]);
        }

        public string SelectedIcon {
            get => this.selectedIconUri;
            set => this.SetPropertyValue(ref this.selectedIconUri, value, [nameof(this.SelectedIcon), nameof(this.EffectiveSelectedIcon), nameof(this.IconPath),]);
        }

        public string EffectiveSelectedIcon => string.IsNullOrEmpty(this.SelectedIcon) ? this.Icon : this.SelectedIcon;

        public object ToolTip {
            get => this.tooltip;
            set => this.SetPropertyValue(ref this.tooltip, value, nameof(this.ToolTip));
        }

        public ICommand Command
        {
            get => this.command;
            set => this.SetPropertyValue(ref this.command, value, nameof(this.Command));
        }

        public object CommandParameter
        {
            get => this.commandParameter;
            set => this.SetPropertyValue(ref this.commandParameter, value, nameof(this.CommandParameter));
        }

        public bool IsSelected
        {
            get => this.isSelected;
            set => this.SetPropertyValue(ref this.isSelected, value, [nameof(this.IsSelected), nameof(this.IconPath)]);
        }

        public bool ShowSelectedIcon
        {
            get => this.showSelectedIcon;
            set => this.SetPropertyValue(ref this.showSelectedIcon, value, [nameof(this.ShowSelectedIcon), nameof(this.IconPath)]);
        }

        public string IconPath => this.IsSelected || this.ShowSelectedIcon
                ? this.EffectiveSelectedIcon
                : this.Icon;

        public bool IsVisible
        {
            get => this.isVisible;
            set => this.SetPropertyValue(ref this.isVisible, value, nameof(this.IsVisible));
        }

        public FontWeight FontWeight
        {
            get => this.fontWeight;
            set => this.SetPropertyValue(ref this.fontWeight, value, nameof(this.FontWeight));
        }

        public Brush Foreground
        {
            get => this.foreground;
            set => this.SetPropertyValue(ref this.foreground, value, nameof(this.Foreground));
        }

        public Brush Background
        {
            get => this.background;
            set => this.SetPropertyValue(ref this.background, value, nameof(this.Background));
        }

        public Brush MouseOver
        {
            get => this.mouseOver;
            set => this.SetPropertyValue(ref this.mouseOver, value, nameof(this.MouseOver));
        }

        public Brush Pressed
        {
            get => this.pressed;
            set => this.SetPropertyValue(ref this.pressed, value, nameof(this.Pressed));
        }

        /// <summary>
        /// Arbitrary metadata for the navigation item.
        /// Typically used to store the destination view type.
        /// </summary>
        public object Tag
        {
            get => this.tag;
            set => this.SetPropertyValue(ref this.tag, value, nameof(this.Tag));
        }

        public CornerRadius CornerRadius
        {
            get => this.cornerRadius;
            set => this.SetPropertyValue(ref this.cornerRadius, value, nameof(this.CornerRadius));
        }

        public override string ToString()
        {
            return this.Label ?? base.ToString();
        }

        #region Static Creation Helpers

        public static NavigationItem Create(string label, string iconUrl)
        {
            return new NavigationItem()
            {
                Label = label,
                Icon = iconUrl,
            };
        }

        public static NavigationItem Create(string label, string iconUrl, string selectedIconUri)
        {
            return new NavigationItem()
            {
                Label = label,
                Icon = iconUrl,
                SelectedIcon = selectedIconUri,
            };
        }

        public static NavigationItem Create(string label, string iconUrl, string selectedIconUri, object toolTip)
        {
            return new NavigationItem()
            {
                Label = label,
                Icon = iconUrl,
                SelectedIcon = selectedIconUri,
                ToolTip = toolTip,
            };
        }

        #endregion
    }
}
