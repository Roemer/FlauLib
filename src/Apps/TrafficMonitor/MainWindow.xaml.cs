using System.Windows;
using TrafficMonitor.Views;

namespace TrafficMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private QuickDisplayView _quickView = new QuickDisplayView(); 
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            DataContext = vm;

            _quickView.DataContext = vm;
            _quickView.Show();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            _quickView.Close();
            base.OnClosing(e);
        }
    }
}
