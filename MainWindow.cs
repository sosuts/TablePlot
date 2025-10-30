using System.Windows;
using TablePlots.ViewModels2;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace TablePlots
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel2();
        }

        // DataGrid の自動生成列イベントハンドラ
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // 列の最小幅を設定（Excel のように空欄でも列が存在するようにする）
            e.Column.MinWidth = 90;

            // 列幅のデフォルト振る舞いを明示的に設定する（必要なら固定幅にもできる）
            // e.Column.Width = new DataGridLength(120); // 固定幅にする場合
            // または e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            // バインディング列の場合、空値でも見えるようにプレースホルダを指定
            if (e.Column is DataGridBoundColumn boundCol)
            {
                if (boundCol.Binding is Binding b)
                {
                    // null のときやバインド失敗時に代替表示（空文字だと薄く折りたたまれる事があるので空白を入れる）
                    b.TargetNullValue = " ";
                    b.FallbackValue = " ";
                }

                // 列内表示要素のスタイル（TextBlock）の最小幅を保証する
                var elementStyle = new Style(typeof(TextBlock));
                elementStyle.Setters.Add(new Setter(TextBlock.MinWidthProperty, 90.0));
                elementStyle.Setters.Add(new Setter(TextBlock.PaddingProperty, new Thickness(6, 4, 6, 4)));
                elementStyle.Setters.Add(new Setter(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis));
                boundCol.ElementStyle = elementStyle;
            }

            // ヘッダーやセルの罫線が見づらければ列の Border を調整
            // （例: ヘッダー側は ColumnHeaderStyle で既に設定しているはず）
        }
    }
}