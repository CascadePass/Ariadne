using CascadePass.CascadeCore.UI;
using System.Collections.ObjectModel;

namespace CascadePass.DatabaseSupernova.UI
{
    public class WorkspaceViewModel : ViewModel
    {
        private bool isSearchVisible, isNavigationTrayExpanded;
        private ObservableCollection<NavigationItem> navOptions;
        private NavigationItem settingsNavigationItem;

        public WorkspaceViewModel()
        {
            this.NavigationCommands = new() { WorkspaceViewModel = this, };

            this.NavigationOptions = [];
            this.CreateNavigationItems();
        }

        public NavigationCommands NavigationCommands { get; }

        public bool IsSearchVisible
        {
            get => this.isSearchVisible;
            set => this.SetPropertyValue(ref this.isSearchVisible, value, nameof(this.IsSearchVisible));
        }

        public bool IsNavigationTrayExpanded
        {
            get => this.isNavigationTrayExpanded;
            set => this.SetPropertyValue(ref this.isNavigationTrayExpanded, value, nameof(this.IsNavigationTrayExpanded));
        }

        public ObservableCollection<NavigationItem> NavigationOptions
        {
            get => navOptions;
            set => this.SetPropertyValue(ref navOptions, value, nameof(this.NavigationOptions));
        }

        public NavigationItem SettingsNavigationItem
        {
            get => this.settingsNavigationItem;
            set => this.SetPropertyValue(ref this.settingsNavigationItem, value, nameof(this.SettingsNavigationItem));
        }

        protected void CreateNavigationItems()
        {
            //TODO: Load this from a configuration file or similar.

            this.SettingsNavigationItem = new NavigationItem
            {
                Label = DisplayText.Settings,
                Icon = "/Images/Navigation/Settings.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Settings.Icon.Selected.png",
                Command = this.NavigationCommands.ShowFeatureCommand,
                CommandParameter = AriadneFeature.Settings,
            };


            this.NavigationOptions.Add(new NavigationItem
            {
                Icon = "/Images/Navigation/Menu.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Menu.Icon.Selected.png",
                Command = this.NavigationCommands.ToggleNavigationTrayExpansionCommand,
            });

            this.NavigationOptions.Add(new NavigationItem
            {
                Label = DisplayText.Home,
                Icon = "/Images/Navigation/Home.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Home.Icon.Selected.png",
                Command = this.NavigationCommands.ShowFeatureCommand,
                CommandParameter = AriadneFeature.Home,
                IsSelected = true,
            });

            this.NavigationOptions.Add(new NavigationItem
            {
                Label = DisplayText.ServiceRemoteControl,
                Icon = "/Images/Navigation/Services.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Services.Icon.Selected.png",
                Command = this.NavigationCommands.ShowFeatureCommand,
                CommandParameter = AriadneFeature.Services,
            });

            this.NavigationOptions.Add(new NavigationItem
            {
                Label = DisplayText.Columns,
                Icon = "/Images/Navigation/Column.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Column.Icon.Selected.png",
                Command = this.NavigationCommands.ShowFeatureCommand,
                CommandParameter = AriadneFeature.Columns,
            });

            this.NavigationOptions.Add(new NavigationItem
            {
                Label = DisplayText.LockSmith,
                Icon = "/Images/Navigation/Locksmith.Icon.Dark.png",
                SelectedIcon = "/Images/Navigation/Locksmith.Icon.Selected.png",
                Command = this.NavigationCommands.ShowFeatureCommand,
                CommandParameter = AriadneFeature.LockSmith,
            });
        }

        public void ShowPage(AriadneFeature feature)
        {
            foreach(var item in this.NavigationOptions)
            {
                if (item.Command is not DelegateCommand command || item.CommandParameter is not AriadneFeature)
                {
                    continue;
                }

                item.IsSelected = (AriadneFeature)item.CommandParameter == feature;
            }

            this.SettingsNavigationItem.IsSelected = feature == AriadneFeature.Settings;
        }
    }
}
