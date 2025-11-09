using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using TablePlots.Models;

namespace TablePlots.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<ValueMeterViewModel> ValueMeters { get; set; }

        // 転置表示用
        public DataView TransposedView { get; private set; }
        public MainViewModel()
        {
            SelectedIndex = 0;
            ValueMeters = new ObservableCollection<ValueMeterViewModel>()
            {
                new ValueMeterViewModel() { Value = 0, GuideLineThickness = 2 },
                new ValueMeterViewModel() { Value = 25, GuideLineThickness = 2 },
                new ValueMeterViewModel() { Value = 50, GuideLineThickness = 2 },
                new ValueMeterViewModel() { Value = 75, GuideLineThickness = 2 },
                new ValueMeterViewModel() { Value = 100, GuideLineThickness = 2 },
            };

            // 転置用サンプル
            var sampleData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Date", "2025-10-01"},
                    {"温度", 2},
                    {"湿度", 1},
                    {"照度", 3},
                    {"気圧", 5},
                    {"騒音", 6}
                },
                new Dictionary<string, object>
                {
                    {"Date", "2025-10-02"},
                    {"温度", 3},
                    {"湿度", 2},
                    {"照度", 4},
                    {"気圧", 6},
                    {"騒音", 7}
                }
            };
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                // デザイン時用のダミーデータ
                TransposedView = BuildTransposedFromList(sampleData);
            }
            else
            {
                TransposedView = BuildTransposedFromList(sampleData);
            }
        }

        // TODO: 戻り値の型を追加（DataView）
        private DataView BuildTransposedFromList(List<Dictionary<string, object>> list)
        {
            if (list == null || list.Count == 0) return null;

            var dates = new List<string>();
            var metrics = new SortedSet<string>();
            foreach (var entry in list)
            {
                if (entry.TryGetValue("Date", out var d)) dates.Add(d?.ToString() ?? string.Empty);
                foreach (var kv in entry)
                {
                    if (kv.Key == "Date") continue;
                    metrics.Add(kv.Key);
                }
            }

            var table = new DataTable();
            table.Columns.Add("項目", typeof(string));
            int dateIndex = 1;
            foreach (var date in dates)
            {
                var col = table.Columns.Add($"測定値{dateIndex} ({date})", typeof(string));
                col.Caption = $"測定値{dateIndex} ({date})";
                dateIndex++;
            }

            foreach (var metric in metrics)
            {
                var row = table.NewRow();
                row[0] = metric;
                for (int i = 0; i < dates.Count; i++)
                {
                    var entry = list[i];
                    row[i + 1] = entry.ContainsKey(metric) ? entry[metric]?.ToString() ?? "" : "";
                }
                table.Rows.Add(row);
            }
            return table.DefaultView;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}