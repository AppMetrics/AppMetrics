namespace App.Metrics.Concurrency.Internal
{
    internal interface IVolatileValue<T> : IValueReader<T>, IValueWriter<T>
    {
    }
}