using System;
using System.Diagnostics;

namespace FlauLib.Tools
{
    public enum NetworkTrafficAnalyzerCounterType
    {
        InLastSec,
        OutLastSec,
    }

    /// <summary>
    /// Class to get available network adapters and get statistics out of them
    /// Last updated: 14.01.2015
    /// </summary>
    public class NetworkTrafficAnalyzer
    {
        private readonly PerformanceCounterCategory _performanceCounterCategory;

        public NetworkTrafficAnalyzer()
        {
            _performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
        }

        public string[] GetNetworkAdapters()
        {
            return _performanceCounterCategory.GetInstanceNames();
        }

        public PerformanceCounter GetCounter(string instanceName, NetworkTrafficAnalyzerCounterType counter)
        {
            var text = CounterToString(counter);
            return new PerformanceCounter("Network Interface", text, instanceName);
        }

        private string CounterToString(NetworkTrafficAnalyzerCounterType counter)
        {
            switch (counter)
            {
                case NetworkTrafficAnalyzerCounterType.InLastSec:
                    return "Bytes Received/sec";
                case NetworkTrafficAnalyzerCounterType.OutLastSec:
                    return "Bytes Sent/sec";
                default:
                    throw new ArgumentOutOfRangeException("counter");
            }
        }
    }
}
