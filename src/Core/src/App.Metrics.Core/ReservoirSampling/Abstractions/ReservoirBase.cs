using System.Collections;
using App.Metrics.Concurrency;

namespace App.Metrics.ReservoirSampling.Abstractions
{
    /// <summary>
    /// Base abstract reservoir
    /// </summary>
    public abstract class ReservoirBase<TValues> : IReservoir
        where TValues : ICollection
    {
        protected TValues ValuesCollection;
        protected AtomicLong Count = new AtomicLong(0);
        protected AtomicDouble Sum = new AtomicDouble(0.0);        
        
        /// <summary>
        ///     Gets the size.
        /// </summary>
        /// <value>
        ///     The size.
        /// </value>
        // ReSharper disable once MemberCanBeProtected.Global
        protected internal virtual int Size
        {
            get
            {
                long totalCount = Count.GetValue();
                int bufferCount = ValuesCollection.Count;
                // total count will be always lower than int.MaxValue, casting is safe 
                return totalCount <= bufferCount ? (int)totalCount : bufferCount;
            }
        }

        protected ReservoirBase(TValues valuesCollection)
        {
            ValuesCollection = valuesCollection;
        }
        
        /// <inheritdoc cref="IReservoir"/>
        public abstract IReservoirSnapshot GetSnapshot(bool resetReservoir);

        /// <inheritdoc />
        public IReservoirSnapshot GetSnapshot() => GetSnapshot(false);

        public abstract void Reset();

        public abstract void Update(long value, string userValue);

        public void Update(long value) => Update(value, default);
    }
}