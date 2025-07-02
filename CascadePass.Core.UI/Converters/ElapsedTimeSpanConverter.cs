using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.Core.UI.Converters
{
    public class ElapsedTimeSpanConverter : IValueConverter
    {
        public ElapsedTimeSpanConverter()
        {
            this.GetCurrentTime = () => DateTime.UtcNow;
        }

        public Func<DateTime> GetCurrentTime { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime since && parameter is null)
            {
                return this.GetCurrentTime() - since;
            }

            if (value is DateTime dateTime1 && parameter is DateTime dateTime2)
            {
                return dateTime1 - dateTime2;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

}
