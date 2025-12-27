using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CascadePass.Core.UI.Controls
{
    /// <summary>
    /// Represents an item displayed in a navigation tray, similar to the
    /// navigation panel in Windows Task Manager. Instances of this class
    /// are supplied by consumers of the control and rendered as buttons.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="NavigationItem"/> is an observable model object used by the
    /// navigation tray control to generate interactive navigation buttons.
    /// </para>
    /// <para>
    /// Because this class derives from <see cref="ObservableObject"/> (or your
    /// equivalent base class), UI elements automatically update when property
    /// values change.
    /// </para>
    /// </remarks>
    public class NavigationItem : Observable
    {
        #region Private fields

        private string textLabel;
        private string iconUri, selectedIconUri;
        private bool isSelected, showSelectedIcon, isVisible, isEnabled;
        private object tooltip, tag, commandParameter;
        private ICommand command;

        private FontWeight fontWeight;
        private Brush foreground, background, mouseOver, pressed;
        private CornerRadius cornerRadius;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationItem"/> class
        /// with sensible default visual and behavioral settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// New items are visible and enabled by default, and use a normal
        /// <see cref="FontWeight"/> unless otherwise specified.
        /// </para>
        /// <para>
        /// These defaults ensure that newly created items behave predictably
        /// and require minimal configuration for common scenarios.
        /// </para>
        /// </remarks>
        public NavigationItem()
        {
            this.isVisible = true;
            this.isEnabled = true;
            this.fontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// Gets or sets the text displayed on the navigation button.
        /// </summary>
        public string Label
        {
            get => this.textLabel;
            set => this.SetPropertyValue(ref this.textLabel, value, nameof(this.Label));
        }

        /// <summary>
        /// Gets or sets the icon shown next to the label. This is typically
        /// a <see cref="ImageSource"/> or a resource key referencing an icon.
        /// </summary>
        public string Icon {
            get => this.iconUri;
            set => this.SetPropertyValue(ref this.iconUri, value, [nameof(this.Icon), nameof(this.EffectiveSelectedIcon), nameof(this.IconPath),]);
        }

        /// <summary>
        /// Gets or sets the URI of the icon displayed when the item is selected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If this property is not set, the navigation tray falls back to
        /// <see cref="EffectiveSelectedIcon"/>, which resolves to the normal
        /// <see cref="Icon"/> value.
        /// </para>
        /// <para>
        /// Changing this property also raises notifications for
        /// <see cref="EffectiveSelectedIcon"/> and <c>IconPath</c>
        /// so the UI can update accordingly.
        /// </para>
        /// </remarks>
        public string SelectedIcon
        {
            get => this.selectedIconUri;
            set => this.SetPropertyValue(
                ref this.selectedIconUri,
                value,
                [
            nameof(this.SelectedIcon),
            nameof(this.EffectiveSelectedIcon),
            nameof(this.IconPath),
                ]);
        }

        /// <summary>
        /// Gets the icon URI that should be used when the item is selected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If <see cref="SelectedIcon"/> is null or empty, this property returns
        /// the value of <see cref="Icon"/>. This ensures that the navigation tray
        /// always has a valid icon to display for the selected state.
        /// </para>
        /// </remarks>
        public string EffectiveSelectedIcon =>
            string.IsNullOrEmpty(this.SelectedIcon) ? this.Icon : this.SelectedIcon;

        /// <summary>
        /// Gets or sets the tooltip content displayed when the user hovers over
        /// the navigation item.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property accepts either a simple text value or a fully composed
        /// WPF element, allowing callers to provide rich tooltips containing
        /// images, formatted text, or custom layouts.
        /// </para>
        /// <para>
        /// The navigation tray displays the content as-is, without modification.
        /// </para>
        /// </remarks>
        public object ToolTip
        {
            get => this.tooltip;
            set => this.SetPropertyValue(ref this.tooltip, value, nameof(this.ToolTip));
        }

        /// <summary>
        /// Gets or sets the command executed when the user selects the item.
        /// </summary>
        public ICommand Command
        {
            get => this.command;
            set => this.SetPropertyValue(ref this.command, value, nameof(this.Command));
        }

        /// <summary>
        /// Gets or sets an optional parameter passed to <see cref="Command"/>
        /// when the item is invoked.
        /// </summary>
        public object CommandParameter
        {
            get => this.commandParameter;
            set => this.SetPropertyValue(ref this.commandParameter, value, nameof(this.CommandParameter));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the navigation item is
        /// currently selected. The UI may use this to highlight the item.
        /// </summary>
        public bool IsSelected
        {
            get => this.isSelected;
            set => this.SetPropertyValue(ref this.isSelected, value, [nameof(this.IsSelected), nameof(this.IconPath)]);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the navigation tray should
        /// display a distinct icon when the item is selected.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When set to <c>true</c>, the tray uses <see cref="SelectedIcon"/> (or
        /// <see cref="EffectiveSelectedIcon"/> if a fallback is needed) while the
        /// item is selected.
        /// </para>
        /// <para>
        /// When set to <c>false</c>, the tray uses the normal <see cref="Icon"/>
        /// for both selected and unselected states. This is useful when callers
        /// only provide a single icon and do not require a visual distinction.
        /// </para>
        /// <para>
        /// Changing this property also raises a notification for
        /// <c>IconPath</c> so the UI can update its displayed icon.
        /// </para>
        /// </remarks>
        public bool ShowSelectedIcon
        {
            get => this.showSelectedIcon;
            set => this.SetPropertyValue(
                ref this.showSelectedIcon,
                value,
                [
            nameof(this.ShowSelectedIcon),
            nameof(this.IconPath),
                ]);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// Disabled items are rendered but cannot be interacted with.
        /// </summary>
        public bool IsEnabled
        {
            get => this.isEnabled;
            set => this.SetPropertyValue(ref this.isEnabled, value, nameof(this.IsEnabled));
        }

        /// <summary>
        /// Gets the URI of the icon that should be displayed for this item
        /// based on its current selection state and configuration.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property determines which icon the navigation tray should render.
        /// If the item is selected, or if <see cref="ShowSelectedIcon"/> is
        /// <c>true</c>, the value of <see cref="EffectiveSelectedIcon"/> is used.
        /// Otherwise, the normal <see cref="Icon"/> is returned.
        /// </para>
        /// <para>
        /// This ensures that callers who provide only a single icon can opt to
        /// reuse it for both states, while callers who supply a distinct
        /// <see cref="SelectedIcon"/> can enable visual differentiation.
        /// </para>
        /// </remarks>
        public string IconPath =>
            this.IsSelected || this.ShowSelectedIcon
                ? this.EffectiveSelectedIcon
                : this.Icon;

        /// <summary>
        /// Gets or sets a value indicating whether this navigation item is
        /// visible in the navigation tray.
        /// </summary>
        /// <remarks>
        /// When set to <c>false</c>, the item is not rendered but remains part
        /// of the underlying collection.
        /// </remarks>
        public bool IsVisible
        {
            get => this.isVisible;
            set => this.SetPropertyValue(ref this.isVisible, value, nameof(this.IsVisible));
        }

        /// <summary>
        /// Gets or sets the font weight used to render the item's label.
        /// </summary>
        /// <remarks>
        /// This allows callers to emphasize specific items (for example,
        /// using <see cref="FontWeights.Bold"/>).
        /// </remarks>
        public FontWeight FontWeight
        {
            get => this.fontWeight;
            set => this.SetPropertyValue(ref this.fontWeight, value, nameof(this.FontWeight));
        }

        /// <summary>
        /// Gets or sets the brush used to render the item's foreground content,
        /// typically the label text and icon tint.
        /// </summary>
        /// <remarks>
        /// Callers may supply any WPF <see cref="Brush"/>, including solid colors,
        /// gradients, or dynamic resources.
        /// </remarks>
        public Brush Foreground
        {
            get => this.foreground;
            set => this.SetPropertyValue(ref this.foreground, value, nameof(this.Foreground));
        }

        /// <summary>
        /// Gets or sets the background brush used to render the navigation item.
        /// </summary>
        /// <remarks>
        /// This brush defines the item's normal (non‑hover, non‑pressed) background.
        /// </remarks>
        public Brush Background
        {
            get => this.background;
            set => this.SetPropertyValue(ref this.background, value, nameof(this.Background));
        }

        /// <summary>
        /// Gets or sets the background brush applied when the user hovers
        /// over the navigation item.
        /// </summary>
        /// <remarks>
        /// This allows callers to customize hover feedback to match their
        /// application's visual style.
        /// </remarks>
        public Brush MouseOver
        {
            get => this.mouseOver;
            set => this.SetPropertyValue(ref this.mouseOver, value, nameof(this.MouseOver));
        }

        /// <summary>
        /// Gets or sets the background brush applied when the navigation item
        /// is pressed or clicked.
        /// </summary>
        /// <remarks>
        /// This brush is used to provide visual feedback during pointer or
        /// keyboard activation.
        /// </remarks>
        public Brush Pressed
        {
            get => this.pressed;
            set => this.SetPropertyValue(ref this.pressed, value, nameof(this.Pressed));
        }

        /// <summary>
        /// Gets or sets the corner radius applied to the navigation item's
        /// visual container.
        /// </summary>
        /// <remarks>
        /// This allows callers to control the roundness of the item's edges,
        /// enabling styles ranging from sharp rectangles to fully rounded pills.
        /// </remarks>
        public CornerRadius CornerRadius
        {
            get => this.cornerRadius;
            set => this.SetPropertyValue(ref this.cornerRadius, value, nameof(this.CornerRadius));
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

        /// <summary>
        /// Returns a string representation of the navigation item.
        /// </summary>
        /// <returns>
        /// The value of <see cref="Label"/> if it is set; otherwise, the base
        /// implementation's string representation.
        /// </returns>
        /// <remarks>
        /// This override improves debugging and design‑time tooling by showing
        /// the item's label instead of a type name.
        /// </remarks>
        public override string ToString()
        {
            return this.Label ?? base.ToString();
        }

        #region Static Creation Helpers

        /// <summary>
        /// Creates a new <see cref="NavigationItem"/> with the specified label
        /// and icon.
        /// </summary>
        /// <param name="label">
        /// The text displayed on the navigation button.
        /// </param>
        /// <param name="iconUrl">
        /// The URI of the icon shown for both selected and unselected states.
        /// </param>
        /// <returns>
        /// A new <see cref="NavigationItem"/> initialized with the provided values.
        /// </returns>
        /// <remarks>
        /// This overload is intended for callers who use a single icon for all
        /// visual states.
        /// </remarks>
        public static NavigationItem Create(string label, string iconUrl)
        {
            return new NavigationItem()
            {
                Label = label,
                Icon = iconUrl,
            };
        }

        /// <summary>
        /// Creates a new <see cref="NavigationItem"/> with the specified label,
        /// icon, and selected-state icon.
        /// </summary>
        /// <param name="label">
        /// The text displayed on the navigation button.
        /// </param>
        /// <param name="iconUrl">
        /// The URI of the icon shown when the item is not selected.
        /// </param>
        /// <param name="selectedIconUri">
        /// The URI of the icon shown when the item is selected.
        /// </param>
        /// <returns>
        /// A new <see cref="NavigationItem"/> initialized with the provided values.
        /// </returns>
        /// <remarks>
        /// This overload is useful when callers want a distinct visual appearance
        /// for the selected state.
        /// </remarks>
        public static NavigationItem Create(string label, string iconUrl, string selectedIconUri)
        {
            return new NavigationItem()
            {
                Label = label,
                Icon = iconUrl,
                SelectedIcon = selectedIconUri,
            };
        }

        /// <summary>
        /// Creates a new <see cref="NavigationItem"/> with the specified label,
        /// icons, and tooltip content.
        /// </summary>
        /// <param name="label">
        /// The text displayed on the navigation button.
        /// </param>
        /// <param name="iconUrl">
        /// The URI of the icon shown when the item is not selected.
        /// </param>
        /// <param name="selectedIconUri">
        /// The URI of the icon shown when the item is selected.
        /// </param>
        /// <param name="toolTip">
        /// The tooltip content displayed when the user hovers over the item.
        /// May be a simple string or a fully composed WPF element.
        /// </param>
        /// <returns>
        /// A new <see cref="NavigationItem"/> initialized with the provided values.
        /// </returns>
        /// <remarks>
        /// This overload provides the most flexibility, allowing callers to supply
        /// rich tooltip content in addition to custom icons.
        /// </remarks>
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
