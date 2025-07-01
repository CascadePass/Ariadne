using CascadePass.CascadeCore.UI;
using System.Windows.Input;

namespace CascadePass.DatabaseSupernova.UI
{
    public class NavigationItem : ViewModel
    {
        private bool isSelected, showSelectedIcon;

        public string Label { get; set; }
        public string Icon { get; set; }
        public string SelectedIcon { get; set; }
        public string ToolTipText { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set => this.SetPropertyValue(ref this.isSelected, value, [nameof(this.IsSelected), nameof(this.IconPath)]);
        }

        public bool ShowSelectedIcon
        {
            get => showSelectedIcon;
            set => this.SetPropertyValue(ref this.showSelectedIcon, value, [nameof(this.ShowSelectedIcon), nameof(this.IconPath)]);
        }

        public string IconPath => this.IsSelected || this.ShowSelectedIcon
                ? this.SelectedIcon
                : this.Icon;
    }
}
