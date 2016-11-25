namespace App.Metrics.Concurrency.Internal
{
    internal interface IAtomicValue<T> : IVolatileValue<T>
    {
        T Add(T value);

        bool CompareAndSwap(T expected, T updated);

        T Decrement();

        T Decrement(T value);

        T GetAndAdd(T value);

        T GetAndDecrement();

        T GetAndDecrement(T value);

        T GetAndIncrement();

        T GetAndIncrement(T value);

        T GetAndReset();

        T GetAndSet(T newValue);

        T Increment();

        T Increment(T value);
    }
}