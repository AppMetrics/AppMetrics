using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using App.Metrics.Core;
using App.Metrics.Utils;
using Microsoft.Extensions.Logging;

namespace App.Metrics
{
    /// <summary>
    ///     Static wrapper around a global MetricContext instance.
    /// </summary>
    public static class Metric
    {
        private static readonly DefaultMetricsContext globalContext;
        //TODO: AH - Inject Logger
        private static readonly ILogger Log = new LoggerFactory().CreateLogger(typeof(Metric));

        static Metric()
        {
            globalContext = new DefaultMetricsContext(GetGlobalContextName());
            if (MetricsConfig.GloballyDisabledMetrics)
            {
                globalContext.CompletelyDisableMetrics();
                Log.LogInformation("Metrics: Metrics.NET Library is completely disabled. Set Metrics.CompletelyDisableMetrics to false to re-enable.");
            }
            Config = new MetricsConfig(globalContext);
            Config.ApplySettingsFromConfigFile();
        }

        /// <summary>
        ///     Exposes advanced operations that are possible on this metrics context.
        /// </summary>
        public static AdvancedMetricsContext Advanced => globalContext;

        /// <summary>
        ///     Entrypoint for Global Metrics Configuration.
        /// </summary>
        /// <example>
        ///     <code>
        /// Metric.Config
        ///     .WithHttpEndpoint("http://localhost:1234/")
        ///     .WithErrorHandler(x => Console.WriteLine(x.ToString()))
        ///     .WithAllCounters()
        ///     .WithReporting(config => config
        ///         .WithConsoleReport(TimeSpan.FromSeconds(30))
        ///         .WithCSVReports(@"c:\temp\reports\", TimeSpan.FromSeconds(10))
        ///         .WithTextFileReport(@"C:\temp\reports\metrics.txt", TimeSpan.FromSeconds(10))
        ///     );
        /// </code>
        /// </example>
        public static MetricsConfig Config { get; }

        internal static MetricsContext Internal { get; } = new DefaultMetricsContext("Metrics.NET");

        /// <summary>
        ///     Create a new child metrics context. Metrics added to the child context are kept separate from the metrics in the
        ///     parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context.</param>
        /// <returns>Newly created child context.</returns>
        public static MetricsContext Context(string contextName)
        {
            return globalContext.Context(contextName);
        }

        /// <summary>
        ///     Create a new child metrics context. Metrics added to the child context are kept separate from the metrics in the
        ///     parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context.</param>
        /// <param name="contextCreator">
        ///     Function used to create the instance of the child context. (Use for creating custom
        ///     contexts)
        /// </param>
        /// <returns>Newly created child context.</returns>
        public static MetricsContext Context(string contextName, Func<string, MetricsContext> contextCreator)
        {
            return globalContext.Context(contextName, contextCreator);
        }

        /// <summary>
        ///     A counter is a simple incrementing and decrementing 64-bit integer. Ex number of active requests.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all counters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="tags">
        ///     Optional set of tags that can be associated with the metric. Tags can be string array or comma separated values in
        ///     a string.
        ///     ex: tags: "tag1,tag2" or tags: new[] {"tag1", "tag2"}
        /// </param>
        /// <returns>Reference to the metric</returns>
        public static Counter Counter(string name, Unit unit, MetricTags tags = default(MetricTags))
        {
            return globalContext.Counter(name, unit, tags);
        }

        /// <summary>
        ///     A gauge is the simplest metric type. It just returns a value. This metric is suitable for instantaneous values.
        /// </summary>
        /// <param name="name">Name of this gauge metric. Must be unique across all gauges in this context.</param>
        /// <param name="valueProvider">Function that returns the value for the gauge.</param>
        /// <param name="unit">Description of want the value represents ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the gauge</returns>
        public static void Gauge(string name, Func<double> valueProvider, Unit unit, MetricTags tags = default(MetricTags))
        {
            globalContext.Gauge(name, valueProvider, unit, tags);
        }

        /// <summary>
        ///     A Histogram measures the distribution of values in a stream of data: e.g., the number of results returned by a
        ///     search.
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all histograms in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="samplingType">Type of the sampling to use (see SamplingType for details ).</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        public static Histogram Histogram(string name, Unit unit, SamplingType samplingType = SamplingType.Default,
            MetricTags tags = default(MetricTags))
        {
            return globalContext.Histogram(name, unit, samplingType, tags);
        }

        /// <summary>
        ///     A meter measures the rate at which a set of events occur, in a few different ways.
        ///     This metric is suitable for keeping a record of now often something happens ( error, request etc ).
        /// </summary>
        /// <remarks>
        ///     The mean rate is the average rate of events. It’s generally useful for trivia,
        ///     but as it represents the total rate for your application’s entire lifetime (e.g., the total number of requests
        ///     handled,
        ///     divided by the number of seconds the process has been running), it doesn’t offer a sense of recency.
        ///     Luckily, meters also record three different exponentially-weighted moving average rates: the 1-, 5-, and 15-minute
        ///     moving averages.
        /// </remarks>
        /// <param name="name">Name of the metric. Must be unique across all meters in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        public static Meter Meter(string name, Unit unit, TimeUnit rateUnit = TimeUnit.Seconds, MetricTags tags = default(MetricTags))
        {
            return globalContext.Meter(name, unit, rateUnit, tags);
        }

        ///     Remove a child context. The metrics for the child context are removed from the MetricsData of the parent context.
        /// </summary>
        /// <param name="contextName">Name of the child context to shutdown.</param>
        public static void ShutdownContext(string contextName)
        {
            globalContext.ShutdownContext(contextName);
        }

        /// <summary>
        ///     A timer is basically a histogram of the duration of a type of event and a meter of the rate of its occurrence.
        ///     <seealso cref="Histogram" /> and <seealso cref="Meter" />
        /// </summary>
        /// <param name="name">Name of the metric. Must be unique across all timers in this context.</param>
        /// <param name="unit">Description of what the is being measured ( Unit.Requests , Unit.Items etc ) .</param>
        /// <param name="samplingType">Type of the sampling to use (see SamplingType for details ).</param>
        /// <param name="rateUnit">Time unit for rates reporting. Defaults to Second ( occurrences / second ).</param>
        /// <param name="durationUnit">Time unit for reporting durations. Defaults to Milliseconds. </param>
        /// <param name="tags">Optional set of tags that can be associated with the metric.</param>
        /// <returns>Reference to the metric</returns>
        public static Timer Timer(string name, Unit unit, SamplingType samplingType = SamplingType.Default,
            TimeUnit rateUnit = TimeUnit.Seconds, TimeUnit durationUnit = TimeUnit.Milliseconds, MetricTags tags = default(MetricTags))
        {
            return globalContext.Timer(name, unit, samplingType, rateUnit, durationUnit, tags);
        }

        internal static void EnableInternalMetrics()
        {
            globalContext.AttachContext("App.Metrics", Internal);
        }

        private static string CleanName(string name)
        {
            return name.Replace('.', '_');
        }

        private static string GetDefaultGlobalContextName()
        {
            return $@"{CleanName(Environment.MachineName)}.{CleanName(Process.GetCurrentProcess().ProcessName)}";
        }

        private static string GetGlobalContextName()
        {
            try
            {
                const string contextNameKey = "Metrics.GlobalContextName";
                // look in the runtime environment first, then in ConfigurationManager.AppSettings
                //TODO: AH - Inject IOptions to get global context name
                var contextNameValue = Environment.GetEnvironmentVariable(contextNameKey); //ConfigurationManager.AppSettings[contextNameKey];
                var name = string.IsNullOrEmpty(contextNameValue) ? GetDefaultGlobalContextName() : ParseGlobalContextName(contextNameValue);
                Log.LogDebug("Metrics: GlobalContext Name set to " + name);
                return name;
            }
            catch (InvalidOperationException)
            {
                // these are thrown by sub functions and will already be logged.
                throw;
            }
            catch (Exception x)
            {
                Log.LogError("Metrics: Error reading config value for Metrics.GlobalContextName", x);
                throw new InvalidOperationException("Invalid Metrics Configuration: Metrics.GlobalContextName must be non empty string", x);
            }
        }

        private static string ParseGlobalContextName(string configName)
        {
            configName = Regex.Replace(configName, @"\$Env\.MachineName\$", CleanName(Environment.MachineName),
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            configName = Regex.Replace(configName, @"\$Env\.ProcessName\$", CleanName(Process.GetCurrentProcess().ProcessName),
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            const string aspMacro = @"\$Env\.AppDomainAppVirtualPath\$";
            if (Regex.IsMatch(configName, aspMacro, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
            {
                configName = Regex.Replace(configName, aspMacro, CleanName(AppEnvironment.ResolveAspSiteName()),
                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            }

            configName = ReplaceRemainingTokens(configName);

            return configName;
        }

        private static string ReplaceRemainingTokens(string configName)
        {
            // look for any tokens of the pattern $Env.<key>$ where <key> is the name of an environment variable or AppSettings variable to read.
            var matches = Regex.Matches(configName, @"\$Env\.(.+?)\$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            foreach (var match in matches.Cast<Match>())
            {
                // we have a match. The second group is the key to look for.
                var key = match.Groups[1];

                if (string.IsNullOrWhiteSpace(key.Value))
                {
                    var msg =
                        $"Metrics: Error substituting Environment tokens in Metrics.GlobalContextName. Found token with no key. Original string {configName}";
                    Log.LogError(msg);
                    throw new InvalidOperationException(msg);
                }

                // first look in the runtime Environment.
                var val = Environment.GetEnvironmentVariable(key.Value);

                //TODO: AH - load from IOptions<>
                //if (string.IsNullOrWhiteSpace(val))
                //{
                //    // next look in ConfigurationManager.AppSettings
                //    val = ConfigurationManager.AppSettings[key.Value];
                //    if (string.IsNullOrWhiteSpace(val))
                //    {
                //        var msg =
                //            $"Metrics: Error substituting Environment tokens in Metrics.GlobalContextName. Found key '{key}' has no value in Environment or AppSettings. Original string {configName}";
                //        Log.LogError(msg);
                //        throw new InvalidOperationException(msg);
                //    }
                //}

                configName = configName.Replace(match.Value, val);
            }

            return configName;
        }
    }
}