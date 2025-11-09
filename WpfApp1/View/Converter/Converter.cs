using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TablePlots.View.Converter
{
    public class ValueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double v)
            {
                return (v > 50)
                    ? new SolidColorBrush(Color.FromRgb(255, 180, 150))
                    : new SolidColorBrush(Color.FromRgb(170, 210, 255));
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
    public class RatioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && parameter is string ratioStr && double.TryParse(ratioStr, out double ratio))
                return height * ratio;
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
