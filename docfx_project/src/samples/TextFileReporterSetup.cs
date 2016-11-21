.AddReporting(factory =>
{
    var textFileSettings = new TextFileReporterSettings
    {
        ReportInterval = TimeSpan.FromSeconds(30),
        FileName = @"C:\metrics\aspnet-sample.txt"
    };

    factory.AddTextFile(textFileSettings);
})