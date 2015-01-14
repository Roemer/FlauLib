using System;
using System.Threading;

namespace FlauLib.Tools
{
    /// <summary>
    /// Thread save random generator
    /// Last updated: 14.01.2015
    /// </summary>
    public static class ThreadsafeRandomGenerator
    {
        private static readonly object LockObject = new object();
        private static readonly Random SeedRandom = new Random();
        private static readonly ThreadLocal<Random> Random = new ThreadLocal<Random>(GenerateRandom);

        public static Random Instance { get { return Random.Value; } }

        public static Random GenerateRandom()
        {
            lock (LockObject)
            {
                return new Random(SeedRandom.Next());
            }
        }
    }
}
