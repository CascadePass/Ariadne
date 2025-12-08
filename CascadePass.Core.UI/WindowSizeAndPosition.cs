using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CascadePass.Core.UI
{
    public static class MonitorHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromRect(ref RECT lprc, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        private const uint MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        public static Rect GetWorkingArea(Rect windowRect)
        {
            RECT rect = new RECT
            {
                Left = (int)windowRect.Left,
                Top = (int)windowRect.Top,
                Right = (int)windowRect.Right,
                Bottom = (int)windowRect.Bottom
            };

            IntPtr monitor = MonitorFromRect(ref rect, MONITOR_DEFAULTTONEAREST);

            MONITORINFO info = new MONITORINFO();
            info.cbSize = Marshal.SizeOf(info);

            if (GetMonitorInfo(monitor, ref info))
            {
                return new Rect(
                    info.rcWork.Left,
                    info.rcWork.Top,
                    info.rcWork.Right - info.rcWork.Left,
                    info.rcWork.Bottom - info.rcWork.Top);
            }

            // Fallback: primary screen
            return SystemParameters.WorkArea;
        }
    }

    public class WindowSizeAndPosition
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public WindowState WindowState { get; set; }

        public void UpdateFromWindow(Window window)
        {
            if (window is null)
            {
                return;
            }

            this.WindowState = window.WindowState;

            if (window.WindowState == WindowState.Normal)
            {
                this.Width = window.Width;
                this.Height = window.Height;
                this.Left = window.Left;
                this.Top = window.Top;
            }
        }

        public static WindowSizeAndPosition FromWindow(Window window)
        {
            if (window is null)
            {
                return null;
            }

            return new WindowSizeAndPosition
            {
                Width = window.Width,
                Height = window.Height,
                Left = window.Left,
                Top = window.Top,
                WindowState = window.WindowState
            };
        }

        public void ApplyToWindow(Window window)
        {
            if (window is null) return;

            if (this.WindowState == WindowState.Normal)
            {
                var windowRect = new Rect(this.Left, this.Top, this.Width, this.Height);
                var screen = MonitorHelper.GetWorkingArea(windowRect);

                // Clamp position
                var left = Math.Max(screen.Left, this.Left);
                var top = Math.Max(screen.Top, this.Top);

                // Clamp size
                var width = Math.Min(this.Width, screen.Width);
                var height = Math.Min(this.Height, screen.Height);

                window.Left = left;
                window.Top = top;
                window.Width = width;
                window.Height = height;
            }

            // Always restore the state
            window.WindowState = this.WindowState;
        }
    }
}
