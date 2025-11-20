using CascadePass.Core.UI;
using System.Linq;

namespace CascadePass.Ariadne.UI
{
    public class AriadneNavigationCommands
    {
        //private DelegateCommand toggleNavigationTrayExpansionCommand;
        private DelegateCommand showFeatureCommand;

        public WorkspaceViewModel WorkspaceViewModel { get; set; }

        //public DelegateCommand ToggleNavigationTrayExpansionCommand => this.toggleNavigationTrayExpansionCommand ??= new(this.ToggleNavigationTrayExpansionImplementation);

        public DelegateCommand ShowFeatureCommand => this.showFeatureCommand ??= new(this.ShowFeatureImplementation);

        //protected void ToggleNavigationTrayExpansionImplementation()
        //{
        //    this.WorkspaceViewModel.IsNavigationTrayExpanded = !this.WorkspaceViewModel.IsNavigationTrayExpanded;

        //    var navItem = this.WorkspaceViewModel.NavigationOptions.FirstOrDefault(item => item.Command == this.ToggleNavigationTrayExpansionCommand);

        //    if (navItem is not null)
        //    {
        //        navItem.ShowSelectedIcon = this.WorkspaceViewModel.IsNavigationTrayExpanded;
        //    }
        //}

        protected void ShowFeatureImplementation(object state)
        {
            if (state is AriadneFeature feature)
            {
                this.WorkspaceViewModel.ShowPage(feature);
            }
        }
    }
}
