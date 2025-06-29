using CascadePass.CascadeCore.UI;

namespace CascadePass.DatabaseSupernova.UI
{
    public class WorkspaceViewModel : ViewModel
    {
        private bool isSearchVisible;

        public bool IsSearchVisible
        {
            get => isSearchVisible;
            set => this.SetPropertyValue(ref isSearchVisible, value, nameof(this.IsSearchVisible));
        }
    }
}
