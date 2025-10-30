using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace TablePlots
{
    public partial class ValueMeter : UserControl
    {
        public ValueMeter()
        {
            InitializeComponent();
            Loaded += (sender, e) => UpdateFill();
            SizeChanged += (sender, e) => UpdateFill();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(ValueMeter),
                new PropertyMetadata(0.0, OnValueChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ValueMeter)d).UpdateFill();
        }

        private void UpdateFill()
        {
            if (ActualHeight == 0) return;

            double v = Math.Max(0, Math.Min(100, Value)); // clamp 0–100
            double ratio = v / 100.0;

            FillRect.Height = ratio * ActualHeight;

            FillRect.Fill = (v > 50)
                ? new SolidColorBrush(Color.FromRgb(255, 180, 150))  // 赤系
                : new SolidColorBrush(Color.FromRgb(170, 210, 255)); // 青系
        }
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