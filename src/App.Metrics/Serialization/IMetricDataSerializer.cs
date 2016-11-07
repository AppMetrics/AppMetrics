namespace App.Metrics.Serialization
{
    public interface IMetricDataSerializer
    {
        T Deserialize<T>(string json);

        string Serialize<T>(T value);
    }
}