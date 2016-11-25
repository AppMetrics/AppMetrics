namespace App.Metrics.Concurrency.Internal
{
    internal interface IValueAdder<T> : IValueReader<T>
    {
        void Add(T value);

        void Decrement();

        void Decrement(T value);

        T GetAndReset();

        void Increment();

        void Increment(T value);

        void Reset();
    }
}