using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CascadePass.DatabaseSupernova.UI
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        #region External

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern int TrackPopupMenuEx(
            IntPtr hMenu,
            uint uFlags,
            int x,
            int y,
            IntPtr hwnd,
            IntPtr lptpm);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const uint TPM_LEFTALIGN = 0x0000;
        const uint TPM_RETURNCMD = 0x0100;
        const uint TPM_RIGHTBUTTON = 0x0002;
        const uint WM_SYSCOMMAND = 0x0112;

        #endregion

        public TitleBar()
        {
            this.InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window?.Close();
        }

        private void MaxRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(window);
                MaxRestoreButton.Content = "🗖"; // Maximize icon
            }
            else
            {
                SystemCommands.MaximizeWindow(window);
                MaxRestoreButton.Content = "🗗"; // Restore icon
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window != null)
            {
                SystemCommands.MinimizeWindow(window);
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Released)
            {
                base.OnMouseRightButtonUp(e);

                var window = Window.GetWindow(this);
                var point = PointToScreen(e.GetPosition(this));
                var hWnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
                var hMenu = GetSystemMenu(hWnd, false);

                int cmd = TrackPopupMenuEx(hMenu, TPM_LEFTALIGN | TPM_RETURNCMD | TPM_RIGHTBUTTON,
                                           (int)point.X, (int)point.Y,
                                           hWnd, IntPtr.Zero);

                if (cmd != 0)
                {
                    SendMessage(hWnd, WM_SYSCOMMAND, (IntPtr)cmd, IntPtr.Zero);
                }

            }
        }
    }
}
