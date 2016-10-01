// Written by Iulian Margarintescu
// 
// Ported to .NET Standard Library by Allan Hardy
// Original repo: https://github.com/etishor/Metrics.NET

namespace App.Metrics.Sampling
{
    public interface Reservoir
    {
        Snapshot GetSnapshot(bool resetReservoir = false);

        void Reset();

        void Update(long value, string userValue = null);
    }
}