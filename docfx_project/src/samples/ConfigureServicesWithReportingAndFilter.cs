private static void ConfigureMetrics(IServiceCollection services)
{
	services
		.AddMetrics(options =>
		{                    
			options.ReportingEnabled = true;
			options.GlobalTags.Add("env", "uat");
		})
		.AddReporting(factory =>
		{
			var filter = new DefaultMetricsFilter()
				.WhereType(MetricType.Counter)
				.WhereMetricTaggedWith("filter-tag1", "filter-tag2")
				.WithHealthChecks(true)
				.WithEnvironmentInfo(true);

			factory.AddConsole(new ConsoleReporterSettings
			{
				ReportInterval = TimeSpan.FromSeconds(10),
			}, filter);
		});
}