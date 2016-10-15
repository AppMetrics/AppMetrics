namespace App.Metrics
{
    public interface IMetricsEnvironment
    {
        string ApplicationName { get; }

        string ApplicationVersion { get; }

        string RuntimeFramework { get; }

        string RuntimeFrameworkVersion { get; }
    }
}