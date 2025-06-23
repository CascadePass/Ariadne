using System;
using System.Globalization;
using System.Windows.Data;

namespace CascadePass.CascadeCore.UI.Converters
{
    public class BooleanToOppositeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                // Return the opposite of the boolean value
                return !booleanValue;
            }

            throw new ArgumentException("Value must be a boolean", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                // Return the opposite of the boolean value
                return !booleanValue;
            }

            throw new ArgumentException("Value must be a boolean", nameof(value));
        }
    }
}
