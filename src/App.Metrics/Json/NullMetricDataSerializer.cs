namespace App.Metrics.Json
{
    internal sealed class NullMetricDataSerializer : IMetricDataSerializer
    {
        public T Deserialize<T>(string json)
        {
            return default(T);
        }

        public string Serialize<T>(T value)
        {
            return string.Empty;
        }
    }
}