using System.Windows.Input;

namespace CascadePass.Core.UI.Controls
{
    public class NavigationItem : ViewModel
    {
        private string textLabel;
        private bool isSelected, showSelectedIcon;
        private object tooltip;

        public string Label
        {
            get => this.textLabel;
            set => this.SetPropertyValue(ref this.textLabel, value, nameof(this.Label));
        }


        public string Icon { get; set; }
        public string SelectedIcon { get; set; }

        public object ToolTip {
            get => this.tooltip;
            set => this.SetPropertyValue(ref this.tooltip, value, nameof(this.ToolTip));
        }

        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }

        public ICommand NavigationCommand { get; set; }

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
                ? this.SelectedIcon
                : this.Icon;
    }
}
