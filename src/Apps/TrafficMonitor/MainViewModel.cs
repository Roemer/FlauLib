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

        public string TotalIn
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string TotalOut
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string CurrentIn
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string CurrentOut
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        private void t_Tick(object sender, EventArgs e)
        {
            TotalIn = TextFormatter.SizeSuffix(analyzer.TotalTrafficIn);
            TotalOut = TextFormatter.SizeSuffix(analyzer.TotalTrafficOut);
            CurrentIn = TextFormatter.SizeSuffix(analyzer.CurrentTrafficIn);
            CurrentOut = TextFormatter.SizeSuffix(analyzer.CurrentTrafficOut);
        }
    }
}
