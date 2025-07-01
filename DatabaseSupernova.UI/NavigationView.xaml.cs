using System.Windows;
using System.Windows.Controls;

namespace CascadePass.DatabaseSupernova.UI
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded", typeof(bool), typeof(NavigationView),
                new PropertyMetadata(false, null));

        public NavigationView()
        {
            InitializeComponent();
        }

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }
    }
}
