using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficMonitor.DesignTime
{
    public static class DesignTimeDataContext
    {
        public static MainViewModel MainViewModel
        {
            get;
            private set;
        }

        static DesignTimeDataContext()
        {
            MainViewModel = new MainViewModel();
            MainViewModel.TotalIn = "5 GB";
            MainViewModel.TotalOut = "500 MB";
            MainViewModel.CurrentIn = "1 GB";
            MainViewModel.CurrentOut = "100 MB";
        }
    }
}
