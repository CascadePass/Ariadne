using System;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.Core.UI.Converters
{
    public class FileSizeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            long longValue = 0;

            if (value is long longFileSize)
            {
                longValue = longFileSize;
            }
            else if (value is int int32FileSize)
            {
                longValue = int32FileSize;
            }
            else if (value is Int16 int16FileSize)
            {
                longValue = int16FileSize;
            }


            // Convert file size to a human-readable format

            if (longValue < 1024)
            {
                return $"{longValue} B";
            }
            else if (longValue < 1024 * 1024)
            {
                return $"{longValue / 1024.0:F2} KB";
            }
            else if (longValue < 1024 * 1024 * 1024)
            {
                return $"{longValue / (1024.0 * 1024):F2} MB";
            }
            else
            {
                return $"{longValue / (1024.0 * 1024 * 1024):F2} GB";
            }

            throw new ArgumentException("Value must be a long representing file size in bytes", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
