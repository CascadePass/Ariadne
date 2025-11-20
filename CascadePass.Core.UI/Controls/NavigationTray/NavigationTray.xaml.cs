using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CascadePass.Core.UI.Controls
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationTray : UserControl
    {
        private DelegateCommand toggleExpansionCommand;
        private Dictionary<NavigationItem, ICommand> navigationCommandMap;

        #region Dependency Properties

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NavigationTray),
                new PropertyMetadata(false, null));

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

        #endregion

        public NavigationTray()
        {
            this.navigationCommandMap = [];
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

        public ICommand ToggleExpansionCommand => this.toggleExpansionCommand ??= new DelegateCommand(this.ToggleTrayExpansion);

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

        #endregion

        #region Methods

        internal static NavigationItem CreateSettingsButton(NavigationTray tray)
        {
            NavigationItem settingsButton = new()
            {
                Label = "Settings",
                Icon = "/Images/Navigation/Settings.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Settings.Icon.Selected.png",
            };

            tray.WrapNavigationItem(settingsButton);

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

        private void WrapNavigationItem(NavigationItem item)
        {
            if (item == null) return;

            var wrapped = new NavigationCommand(
                item,
                this.AllItems,
                selected => SelectedNavigationItem = selected);

            item.NavigationCommand = wrapped;
            navigationCommandMap[item] = wrapped;
        }

        #region Callbacks for Dependency Properties

        private static void OnNavigationItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawer = (NavigationTray)d;

            drawer.navigationCommandMap.Clear();

            if (e.NewValue is ObservableCollection<NavigationItem> items)
            {
                foreach (var item in items)
                {
                    drawer.WrapNavigationItem(item);
                }
            }
        }

        private static void OnSettingsButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var drawer = (NavigationTray)d;

            if (e.NewValue is NavigationItem item)
            {
                drawer.WrapNavigationItem(item);
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
                    option.IsSelected = false;
            }
        }

        #endregion

        #endregion
    }
}
