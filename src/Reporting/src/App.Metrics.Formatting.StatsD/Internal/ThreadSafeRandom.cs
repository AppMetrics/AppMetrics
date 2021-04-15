using System;

namespace App.Metrics.Formatting.StatsD.Internal
{
    internal class ThreadSafeRandom
    {
        private static readonly Random Global = new Random();

        [ThreadStatic] 
        private static Random _local;

        private Random Local
        {
            get
            {
                if (_local == null)
                {
                    int seed;
                    lock (Global)
                    {
                        seed = Global.Next();
                    }

                    _local = new Random(seed);
                }

                return _local;
            }
        }

        public double NextDouble => Local.NextDouble();
    }
}
