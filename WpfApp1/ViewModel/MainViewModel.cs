using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TablePlots.ViewModels
{
    public class RowItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }

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

        public ObservableCollection<RowItem> LeftTable { get; } = new ObservableCollection<RowItem>();
        public ObservableCollection<RowItem> FullTable { get; } = new ObservableCollection<RowItem>();
        public ObservableCollection<RowItem> SummaryTable { get; } = new ObservableCollection<RowItem>();

        // 転置表示用
        public DataView TransposedView { get; private set; }

        // 2D scatter
        public PointCollection ScatterPoints { get; }

        // 3D scatter
        public Point3DCollection Points3D { get; }

        // Line plot
        public PointCollection LinePoints { get; }

        public MainViewModel()
        {
            SelectedIndex = 0;
            // 転置用サンプル
            var sampleData = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"Date", "2025-10-01"},
                    {"温度", 2},
                    {"湿度", 1},
                    {"濃度", 3},
                    {"圧力", 5},
                    {"粘性", 6}
                },
                new Dictionary<string, object>
                {
                    {"Date", "2025-10-02"},
                    {"温度", 3},
                    {"湿度", 2},
                    {"濃度", 4},
                    {"圧力", 6},
                    {"粘性", 7}
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

            // 他のグラフ用データ初期化（必要に応じて）
            ScatterPoints = new PointCollection();
            Points3D = new Point3DCollection();
            LinePoints = new PointCollection();
        }

        // 修正: 戻り値の型を追加（DataView型)
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
            table.Columns.Add("項目名", typeof(string));
            int dateIndex = 1;
            foreach (var date in dates)
            {
                var col = table.Columns.Add($"実験日時{dateIndex}（{date}）", typeof(string));
                col.Caption = $"実験日時{dateIndex}（{date}）";
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
