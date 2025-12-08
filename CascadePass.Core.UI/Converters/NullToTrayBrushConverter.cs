using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CascadePass.Core.UI.Converters
{
    public class NullToTrayBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var itemBrush = values[0] as Brush;
            var trayBrush = values[1] as Brush;
            return itemBrush ?? trayBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class SelectionAwareBrushConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = values[0] is bool b && b;
            var itemBrush = values[1] as Brush;
            var trayBrush = values[2] as Brush;
            var selectedBrush = values[3] as Brush;

            if (isSelected)
                return selectedBrush ?? itemBrush ?? trayBrush;
            return itemBrush ?? trayBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class SelectionAwareFontWeightConverter : IMultiValueConverter
    {
        private static FontWeight? GetFontWeight(object value)
        {
            if (value == null || value == DependencyProperty.UnsetValue)
                return null;

            return value is FontWeight fw ? fw : (FontWeight?)null;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = values[0] is bool b && b;
            FontWeight? item = GetFontWeight(values[1]);          // NavigationButton.Item FontWeight (from NavigationItem)
            FontWeight? trayDefault = GetFontWeight(values[2]);   // NavigationTray.Item default FontWeight (optional)
            FontWeight? traySelected = GetFontWeight(values[3]);  // NavigationTray.SelectedFontWeight

            if (isSelected)
                return traySelected ?? item ?? trayDefault ?? FontWeights.Normal;

            return item ?? trayDefault ?? FontWeights.Normal;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class SelectionAwareCornerRadiusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isSelected = values[0] is bool b && b;
            CornerRadius itemOverride = values[1] as CornerRadius? ?? new CornerRadius(0);
            CornerRadius trayDefault = values[2] as CornerRadius? ?? new CornerRadius(0);
            CornerRadius traySelected = values[3] as CornerRadius? ?? new CornerRadius(0);

            if (isSelected)
            {
                // Prefer item override if set, otherwise tray selected
                return !IsZero(itemOverride) ? itemOverride : traySelected;
            }
            else
            {
                // Prefer item override if set, otherwise tray default
                return !IsZero(itemOverride) ? itemOverride : trayDefault;
            }
        }

        private bool IsZero(CornerRadius cr) =>
            cr.TopLeft == 0 && cr.TopRight == 0 && cr.BottomRight == 0 && cr.BottomLeft == 0;

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
