using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CascadePass.Core.UI.Controls
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationTray : UserControl
    {
        private DelegateCommand toggleExpansionCommand;
        private DelegateCommand _selectAndForward;

        #region Dependency Properties

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NavigationTray),
                new PropertyMetadata(false, OnIsExpandedChanged));

        public static readonly DependencyProperty NavigationItemsProperty =
            DependencyProperty.Register("NavigationItems", typeof(ObservableCollection<NavigationItem>), typeof(NavigationTray),
                new PropertyMetadata(null, OnNavigationItemsChanged));

        public static readonly DependencyProperty ToggleExpansionButtonProperty =
            DependencyProperty.Register("ToggleExpansionButton", typeof(NavigationItem), typeof(NavigationTray),
                new PropertyMetadata(NavigationTray.CreateToggleExpansionButton(), null));

        public static readonly DependencyProperty SettingsButtonProperty =
            DependencyProperty.Register("SettingsButton", typeof(NavigationItem), typeof(NavigationTray),
                new PropertyMetadata(null, OnSettingsButtonChanged));

        public static readonly DependencyProperty SelectedNavigationItemProperty =
            DependencyProperty.Register(
                nameof(SelectedNavigationItem),
                typeof(NavigationItem),
                typeof(NavigationTray),
                new PropertyMetadata(null, OnSelectedNavigationItemChanged));

        public static readonly DependencyProperty SelectionStripeBrushProperty =
            DependencyProperty.Register("SelectionStripeBrush", typeof(Brush), typeof(NavigationTray),
                new PropertyMetadata(Brushes.Red));

        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.Register("ItemForeground", typeof(Brush), typeof(NavigationTray),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ItemCornerRadiusProperty =
            DependencyProperty.Register("ItemCornerRadius", typeof(CornerRadius), typeof(NavigationTray),
                new PropertyMetadata(new CornerRadius(4.0)));

        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.Register(nameof(SelectedForeground), typeof(Brush), typeof(NavigationTray),
                new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register(nameof(SelectedBackground), typeof(Brush), typeof(NavigationTray),
                new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register(nameof(MouseOverBackground), typeof(Brush), typeof(NavigationTray),
                new PropertyMetadata(Brushes.DarkGray));

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(NavigationTray),
                new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty SelectedFontWeightProperty =
            DependencyProperty.Register(nameof(SelectedFontWeight), typeof(FontWeight), typeof(NavigationTray),
                new PropertyMetadata(FontWeights.Bold));

        public static readonly DependencyProperty OpenedWidthProperty =
            DependencyProperty.Register(nameof(OpenedWidth), typeof(double), typeof(NavigationTray),
                new PropertyMetadata(160.0));

        public static readonly DependencyProperty ClosedWidthProperty =
            DependencyProperty.Register(nameof(ClosedWidth), typeof(double), typeof(NavigationTray),
                new PropertyMetadata(60.0));

        public static readonly DependencyProperty OpenCloseDurationProperty =
            DependencyProperty.Register(
                nameof(OpenCloseDuration),
                typeof(Duration),
                typeof(NavigationTray),
                new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(300))));

        #endregion

        public NavigationTray()
        {
            this.InitializeComponent();

            this.SettingsButton ??= NavigationTray.CreateSettingsButton(this);
        }

        #region Properties

        #region Dependency Properties

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public ObservableCollection<NavigationItem> NavigationItems
        {
            get => (ObservableCollection<NavigationItem>)GetValue(NavigationItemsProperty);
            set => SetValue(NavigationItemsProperty, value);
        }

        public NavigationItem ToggleExpansionButton
        {
            get => (NavigationItem)GetValue(ToggleExpansionButtonProperty);
            set => SetValue(ToggleExpansionButtonProperty, value);
        }

        public NavigationItem SettingsButton
        {
            get => (NavigationItem)GetValue(SettingsButtonProperty);
            set => SetValue(SettingsButtonProperty, value);
        }

        public NavigationItem SelectedNavigationItem
        {
            get => (NavigationItem)GetValue(SelectedNavigationItemProperty);
            set => SetValue(SelectedNavigationItemProperty, value);
        }

        public Brush SelectionStripeBrush
        {
            get => (Brush)GetValue(SelectionStripeBrushProperty);
            set => SetValue(SelectionStripeBrushProperty, value);
        }

        public Brush ItemForeground
        {
            get => (Brush)GetValue(ItemForegroundProperty);
            set => SetValue(ItemForegroundProperty, value);
        }

        public Brush SelectedForeground
        {
            get => (Brush)GetValue(SelectedForegroundProperty);
            set => SetValue(SelectedForegroundProperty, value);
        }

        public Brush SelectedBackground
        {
            get => (Brush)GetValue(SelectedBackgroundProperty);
            set => SetValue(SelectedBackgroundProperty, value);
        }

        public Brush MouseOverBackground
        {
            get => (Brush)GetValue(MouseOverBackgroundProperty);
            set => SetValue(MouseOverBackgroundProperty, value);
        }

        public Brush PressedBackground
        {
            get => (Brush)GetValue(PressedBackgroundProperty);
            set => SetValue(PressedBackgroundProperty, value);
        }

        public FontWeight SelectedFontWeight
        {
            get => (FontWeight)GetValue(SelectedFontWeightProperty);
            set => SetValue(SelectedFontWeightProperty, value);
        }

        public CornerRadius ItemCornerRadius
        {
            get => (CornerRadius)GetValue(ItemCornerRadiusProperty);
            set => SetValue(ItemCornerRadiusProperty, value);
        }

        public double OpenedWidth
        {
            get => (double)GetValue(OpenedWidthProperty);
            set => SetValue(OpenedWidthProperty, value);
        }

        public double ClosedWidth
        {
            get => (double)GetValue(ClosedWidthProperty);
            set => SetValue(ClosedWidthProperty, value);
        }

        public Duration OpenCloseDuration
        {
            get => (Duration)GetValue(OpenCloseDurationProperty);
            set => SetValue(OpenCloseDurationProperty, value);
        }

        #endregion

        internal IEnumerable<NavigationItem> AllItems
        {
            get
            {
                if (NavigationItems != null)
                {
                    foreach (var item in NavigationItems)
                        yield return item;
                }

                if (SettingsButton != null)
                    yield return SettingsButton;

                if (ToggleExpansionButton != null)
                    yield return ToggleExpansionButton;
            }
        }

        public ICommand SelectAndForwardCommand => this._selectAndForward ??= new DelegateCommand(ExecuteSelectAndForward);

        public ICommand ToggleExpansionCommand => this.toggleExpansionCommand ??= new DelegateCommand(this.ToggleTrayExpansion);

        #endregion

        #region Methods

        private void ExecuteSelectAndForward(object item)
        {
            if (item is null)
            {
                // Clear selection
                this.SelectedNavigationItem = null;
                return;
            }

            if (item is NavigationItem navigationItem)
            {
                // SelectedNavigationItem has a callback, OnSelectedNavigationItemChanged,
                // that unselects other items.  Just setting this.SelectedNavigationItem
                // is enough to deselect the other NavigationItems individually.

                this.SelectedNavigationItem = navigationItem;
                navigationItem.Command?.Execute(navigationItem.CommandParameter);
            }
        }

        private void AnimateWidth(double targetWidth)
        {
            var animation = new DoubleAnimation
            {
                To = targetWidth,
                Duration = this.OpenCloseDuration,
                AccelerationRatio = 0.2,
                DecelerationRatio = 0.8
            };

            this.NavTray.BeginAnimation(Border.WidthProperty, animation);
        }

        internal static NavigationItem CreateSettingsButton(NavigationTray tray)
        {
            NavigationItem settingsButton = new()
            {
                Label = "Settings",
                Icon = "/Images/Navigation/Settings.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Settings.Icon.Selected.png",
            };

            return settingsButton;
        }

        internal static NavigationItem CreateToggleExpansionButton()
        {
            return new()
            {
                Icon = "/Images/Navigation/Menu.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Menu.Icon.Selected.png",
            };
        }

        public void ToggleTrayExpansion()
        {
            this.IsExpanded = !this.IsExpanded;
            this.ToggleExpansionButton.ShowSelectedIcon = this.IsExpanded;
        }

        #region Callbacks for Dependency Properties

        private static void OnNavigationItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawer = (NavigationTray)d;
        }

        private static void OnSettingsButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawer = (NavigationTray)d;
        }
        
        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawer = (NavigationTray)d;

            var targetWidth = drawer.IsExpanded ? drawer.OpenedWidth : drawer.ClosedWidth;
            if (drawer.OpenCloseDuration == new Duration(TimeSpan.Zero))
            {
                drawer.NavTray.Width = targetWidth;
            }
            else
            {
                drawer.AnimateWidth(targetWidth);
            }
        }

        private static void OnSelectedNavigationItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawer = (NavigationTray)d;

            if (e.NewValue is NavigationItem item)
            {
                foreach (var option in drawer.AllItems)
                {
                    option.IsSelected = item == option;
                }
            }

            if (e.NewValue == null)
            {
                foreach (var option in drawer.AllItems)
                {
                    option.IsSelected = false;
                }
            }
        }

        #endregion

        #endregion
    }
}
