using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ExtensionMethods
{
    public class Profiler
    {
        private Stopwatch stopwatch;
        private int counter;
        private long lastValue;
        private Dictionary<string, long> _meantimes;

        public Dictionary<string, long> Meantimes
        {
            get
            {
                return _meantimes.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public long TotalTime
        {
            get
            {
                return stopwatch.ElapsedMilliseconds;
            }
        }

        public Profiler()
        {
            stopwatch = new Stopwatch();
            _meantimes = new Dictionary<string, long>();
        }

        public void Start()
        {
            counter = 1;
            lastValue = 0;
            _meantimes.Clear();
            stopwatch.Reset();
            stopwatch.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
        }

        public void Break(string label)
        {
            stopwatch.Stop();
            _meantimes[label] = stopwatch.ElapsedMilliseconds - lastValue;
            lastValue = stopwatch.ElapsedMilliseconds;
            stopwatch.Start();
        }

        public void Break()
        {
            Break(counter.ToString());
            counter++;
        }
    }
}
