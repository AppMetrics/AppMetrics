using Newtonsoft.Json;

namespace App.Metrics.Formatters.Json
{
    public class MetricDataSerializer : JsonSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public MetricDataSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new MetricContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
        }

        public virtual T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }

        public virtual string Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
    }
}