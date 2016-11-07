namespace App.Metrics.Serialization
{
    internal sealed class NullHealthStatusSerializer : IHealthStatusSerializer
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