using System.Windows;
using TablePlots.ViewModels;

namespace TablePlots
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

    }
}
