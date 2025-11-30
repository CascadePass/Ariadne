using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.Core.UI.Converters.WindowChrome
{
    public class MaximizeRestoreGlyphConverter : IValueConverter
    {
        public static string RestoreGlyph => "\uE923";

        public static string MaximizeGlyph => "\uE922";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WindowState state)
            {
                return state == WindowState.Maximized ? MaximizeRestoreGlyphConverter.RestoreGlyph : MaximizeRestoreGlyphConverter.MaximizeGlyph;
            }

            return MaximizeRestoreGlyphConverter.MaximizeGlyph;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
