namespace App.Metrics.Json
{
    public class JsonMetric
    {
        private string[] tags = MetricTags.None.Tags;

        public string Name { get; set; }

        public string[] Tags
        {
            get { return tags; }
            set { tags = value ?? new string[0]; }
        }

        public string Unit { get; set; }
    }
}