using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.Core.UI.Converters
{
    public class TextToPlaceholderVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string text)
            {
                return string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
