using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TablePlots.ViewModels
{
    public class ValueMeterViewModel : INotifyPropertyChanged
    {
        private double _value;
        public double Value
        {
            get => _value;
            set { if (_value != value) { _value = value; OnPropertyChanged(); } }
        }

        private double _guideLineThickness = 3.0;
        public double GuideLineThickness
        {
            get => _guideLineThickness;
            set { if (_guideLineThickness != value) { _guideLineThickness = value; OnPropertyChanged(); } }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}