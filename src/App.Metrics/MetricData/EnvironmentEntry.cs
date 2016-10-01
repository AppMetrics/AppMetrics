namespace App.Metrics.MetricData
{
    public struct EnvironmentEntry
    {
        public readonly string Name;
        public readonly string Value;

        public EnvironmentEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}