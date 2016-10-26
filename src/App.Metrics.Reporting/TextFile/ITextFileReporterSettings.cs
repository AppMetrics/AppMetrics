namespace App.Metrics.Reporting.TextFile
{
    public interface ITextFileReporterSettings : IReporterSettings
    {
        string FileReportingFolder { get; }

        //TODO: AH - need to add more params?
    }
}