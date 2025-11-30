using System.Windows;

namespace CascadePass.Core.UI.Behaviors
{
    public static class WindowDragBehavior
    {
        public static readonly DependencyProperty EnableDragProperty =
            DependencyProperty.RegisterAttached("EnableDrag", typeof(bool), typeof(WindowDragBehavior),
                new PropertyMetadata(false, OnEnableDragChanged));

        public static void SetEnableDrag(UIElement element, bool value) =>
            element.SetValue(EnableDragProperty, value);

        public static bool GetEnableDrag(UIElement element) =>
            (bool)element.GetValue(EnableDragProperty);

        private static void OnEnableDragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement ui && (bool)e.NewValue)
            {
                ui.MouseLeftButtonDown += (s, args) =>
                {
                    var window = Window.GetWindow(ui);
                    window?.DragMove();
                };
            }
        }
    }
}
