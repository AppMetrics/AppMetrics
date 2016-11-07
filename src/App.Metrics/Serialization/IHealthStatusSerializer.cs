namespace App.Metrics.Serialization
{
    public interface IHealthStatusSerializer
    {
        T Deserialize<T>(string json);

        string Serialize<T>(T value);
    }
}