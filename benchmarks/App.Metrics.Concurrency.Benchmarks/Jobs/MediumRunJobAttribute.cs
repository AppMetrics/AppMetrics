using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace App.Metrics.Concurrency.Benchmarks.Jobs
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
    public class MediumRunJobAttribute : Attribute, IConfigSource
    {
        /// <summary>
        ///     https://github.com/dotnet/BenchmarkDotNet/blob/307b7250210fc3a7b7ff2c454bb79f793aae57ea/docs/guide/Configs/Jobs.md
        /// </summary>
        public MediumRunJobAttribute()
        {
            var job = Job.MediumRun
                .With(Platform.X64)
                .With(Jit.RyuJit)
                .With(Runtime.Core)                
                .WithId("MediumRun");

            var config = ManualConfig.CreateEmpty();
            config.Add(RPlotExporter.Default);
            config.Add(MarkdownExporter.GitHub);

            Config = config.With(job);
        }

        public IConfig Config { get; }
    }
}