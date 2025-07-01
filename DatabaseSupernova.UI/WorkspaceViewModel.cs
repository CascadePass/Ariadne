using CascadePass.CascadeCore.UI;
using System.Collections.ObjectModel;
using System.Linq;

namespace CascadePass.DatabaseSupernova.UI
{
    public class WorkspaceViewModel : ViewModel
    {
        private bool isSearchVisible, isNavigationTrayExpanded, isSettingsPageVisible;
        private ObservableCollection<NavigationItem> navOptions;

        public WorkspaceViewModel()
        {
            this.NavigationOptions = [];
            this.NavigationCommands = new() { WorkspaceViewModel = this, };
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

        public bool IsSettingsPageVisible
        {
            get => this.isSettingsPageVisible;
            set => this.SetPropertyValue(ref this.isSettingsPageVisible, value, nameof(this.IsSettingsPageVisible));
        }

        public ObservableCollection<NavigationItem> NavigationOptions
        {
            get => navOptions;
            set => this.SetPropertyValue(ref navOptions, value, nameof(this.NavigationOptions));
        }

        protected void CreateNavigationItems()
        {
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

            this.IsSettingsPageVisible = feature == AriadneFeature.Settings;
        }
    }

    public class NavigationCommands
    {
        private DelegateCommand toggleNavigationTrayExpansionCommand;
        private DelegateCommand showFeatureCommand;

        public WorkspaceViewModel WorkspaceViewModel { get; set; }

        public DelegateCommand ToggleNavigationTrayExpansionCommand => this.toggleNavigationTrayExpansionCommand ??= new(this.ToggleNavigationTrayExpansionImplementation);

        public DelegateCommand ShowFeatureCommand => this.showFeatureCommand ??= new(this.ShowFeatureImplementation);

        protected void ToggleNavigationTrayExpansionImplementation()
        {
            this.WorkspaceViewModel.IsNavigationTrayExpanded = !this.WorkspaceViewModel.IsNavigationTrayExpanded;

            var navItem = this.WorkspaceViewModel.NavigationOptions.FirstOrDefault(item => item.Command == this.ToggleNavigationTrayExpansionCommand);

            if (navItem is not null)
            {
                navItem.ShowSelectedIcon = this.WorkspaceViewModel.IsNavigationTrayExpanded;
            }
        }

        protected void ShowFeatureImplementation(object state)
        {
            if (state is AriadneFeature feature)
            {
                this.WorkspaceViewModel.ShowPage(feature);
            }
        }
    }
}
