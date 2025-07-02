using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CascadePass.Core.UI.Converters
{
    public class ElapsedTimeStringConverter : IValueConverter
    {
        public ElapsedTimeStringConverter() {
            this.GetCurrentTime = () => DateTime.UtcNow;
        }

        public Func<DateTime> GetCurrentTime { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not DateTime since)
            {
                return DependencyProperty.UnsetValue;
            }

            DateTime now = this.GetCurrentTime();
            TimeSpan elapsed = now - since;

            return FormatElapsedTime(elapsed);
        }

        private static string FormatElapsedTime(TimeSpan elapsed)
        {
            if (elapsed.TotalSeconds < 60)
                return "just now";
            if (elapsed.TotalMinutes < 2)
                return "a minute ago";
            if (elapsed.TotalMinutes < 60)
                return $"{(int)elapsed.TotalMinutes} minutes ago";
            if (elapsed.TotalHours < 2)
                return "an hour ago";
            if (elapsed.TotalHours < 24)
                return $"{(int)elapsed.TotalHours} hours ago";
            if (elapsed.TotalDays < 2)
                return "yesterday";
            return $"{(int)elapsed.TotalDays} days ago";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            DependencyProperty.UnsetValue;
    }

}
