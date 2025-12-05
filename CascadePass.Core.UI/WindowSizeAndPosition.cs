using System;
using System.Windows;

namespace CascadePass.Core.UI
{
    public class WindowSizeAndPosition
    {
        public double Width { get; set; }

        public double Height { get; set; }

        public double Left { get; set; }

        public double Top { get; set; }

        public void UpdateFromWindow(Window window)
        {
            if (window is null)
            {
                return;
            }

            this.Width = window.Width;
            this.Height = window.Height;
            this.Left = window.Left;
            this.Top = window.Top;
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
            };
        }

        public void ApplyToWindow(Window window)
        {
            if (window is null) return;

            // Default to primary screen bounds
            var screen = SystemParameters.WorkArea;

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
    }
}
