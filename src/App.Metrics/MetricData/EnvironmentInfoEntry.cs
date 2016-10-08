namespace App.Metrics.MetricData
{
    public struct EnvironmentInfoEntry
    {
        public readonly string Name;
        public readonly string Value;

        public EnvironmentInfoEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}