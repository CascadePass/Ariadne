using System;
using System.Globalization;
using System.Windows.Data;

namespace CascadePass.Core.UI.Converters
{
    public class AdditionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double number = System.Convert.ToDouble(value, culture);
                double multiplier = System.Convert.ToDouble(parameter, culture);
                double result = number + multiplier;

                // Optionally convert back to targetType (e.g., int, float)
                return System.Convert.ChangeType(result, targetType, culture);
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
