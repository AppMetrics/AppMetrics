namespace App.Metrics.Concurrency.Internal
{
    internal interface IValueReader<out T>
    {
        T GetValue();

        T NonVolatileGetValue();
    }
}