using FlauLib.MVVM;
using FlauLib.Tools;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace TrafficMonitor
{
    public class MainViewModel : ObservableObject
    {
        private NetworkTrafficAnalyzer analyzer;

        public MainViewModel()
        {
            analyzer = new NetworkTrafficAnalyzer();
            Adapters = new ObservableCollection<string>(analyzer.GetNetworkAdapters());

            var t = new DispatcherTimer();
            t.Interval = TimeSpan.FromSeconds(5);
            t.Tick += t_Tick;
            t.Start();
        }

        public ObservableCollection<string> Adapters { get; private set; }

        public int SelectedItemIndex
        {
            get { return GetProperty<int>(); }
            set
            {
                if (SetProperty(value))
                {
                    analyzer.CurrentAdapter = analyzer.GetNetworkAdapters()[value];
                    t_Tick(null, null);
                }
            }
        }

        public string In
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Out
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            In = TextFormatter.SizeSuffix(analyzer.TotalTrafficIn);
            Out = TextFormatter.SizeSuffix(analyzer.TotalTrafficOut);
        }
    }
}
