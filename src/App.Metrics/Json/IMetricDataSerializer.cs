namespace App.Metrics.Json
{
    public interface IMetricDataSerializer
    {
        T Deserialize<T>(string json);

        string Serialize<T>(T value);
    }
}