using System.Windows;
using System.Windows.Input;

namespace CascadePass.Core.UI.Behaviors
{
    public static class SystemMenuBehavior
    {
        public static readonly DependencyProperty EnableAltSpaceProperty =
            DependencyProperty.RegisterAttached("EnableAltSpace", typeof(bool), typeof(SystemMenuBehavior),
                new PropertyMetadata(false, OnChanged));

        public static void SetEnableAltSpace(UIElement element, bool value) =>
            element.SetValue(EnableAltSpaceProperty, value);

        public static bool GetEnableAltSpace(UIElement element) =>
            (bool)element.GetValue(EnableAltSpaceProperty);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement ui && (bool)e.NewValue)
            {
                ui.PreviewKeyDown += (s, args) =>
                {
                    if (args.SystemKey == Key.Space && Keyboard.Modifiers == ModifierKeys.Alt)
                    {
                        var window = Window.GetWindow(ui);
                        if (window != null)
                        {
                            var point = new Point(window.Left + 10, window.Top + 10);
                            SystemCommands.ShowSystemMenu(window, point);
                            args.Handled = true;
                        }
                    }
                };
            }
        }
    }

}
