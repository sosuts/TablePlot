using System.Windows;
using System.Windows.Controls;

namespace TablePlots.View
{
    public partial class ValueMeter : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ValueMeter), new PropertyMetadata(0.0, OnValuePropertyChanged));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty GuideLineThicknessProperty =
            DependencyProperty.Register("GuideLineThickness", typeof(double), typeof(ValueMeter), new PropertyMetadata(1.0));

        public double GuideLineThickness
        {
            get { return (double)GetValue(GuideLineThicknessProperty); }
            set { SetValue(GuideLineThicknessProperty, value); }
        }

        public ValueMeter()
        {
            InitializeComponent();
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ValueMeter meter)
            {
                meter.UpdateFillHeight();
            }
        }

        private void UpdateFillHeight()
        {
            double percentage = this.Value / 100.0;
            FillRect.Height = this.ActualHeight * percentage;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateFillHeight();
        }
    }
}
