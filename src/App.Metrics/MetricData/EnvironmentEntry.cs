namespace App.Metrics.MetricData
{
    public struct EnvironmentEntry
    {
        public readonly string Name;
        public readonly string Value;

        public EnvironmentEntry(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}