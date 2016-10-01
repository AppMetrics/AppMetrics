

namespace App.Metrics.Sampling
{
    public interface Reservoir
    {
        void Update(long value, string userValue = null);
        Snapshot GetSnapshot(bool resetReservoir = false);
        void Reset();
    }
}
