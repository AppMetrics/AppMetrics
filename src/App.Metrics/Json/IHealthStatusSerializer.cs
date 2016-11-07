namespace App.Metrics.Json
{
    public interface IHealthStatusSerializer
    {
        T Deserialize<T>(string json);

        string Serialize<T>(T value);
    }
}