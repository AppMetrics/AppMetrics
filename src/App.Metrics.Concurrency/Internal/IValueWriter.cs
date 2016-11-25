namespace App.Metrics.Concurrency.Internal
{
    internal interface IValueWriter<in T>
    {
        void LazySetValue(T newValue);

        void NonVolatileSetValue(T newValue);

        void SetValue(T newValue);
    }
}