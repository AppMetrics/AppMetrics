``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300
  [Host] : .NET Core 3.1.4 (CoreCLR 4.700.20.20201, CoreFX 4.700.20.22101), X64 RyuJIT

Runtime=.NET Core 3.1  

```
|                       Method | Mean | Error |
|----------------------------- |-----:|------:|
|     ResolveApdexFromRegistry |   NA |    NA |
|   ResolveCounterFromRegistry |   NA |    NA |
|     ResolveGaugeFromRegistry |   NA |    NA |
| ResolveHistogramFromRegistry |   NA |    NA |
|     ResolveMeterFromRegistry |   NA |    NA |
|     ResolveTimerFromRegistry |   NA |    NA |

Benchmarks with issues:
  DefaultMetricContextRegistryBenchmark.ResolveApdexFromRegistry: Job-IMUXWQ(Runtime=.NET Core 3.1)
  DefaultMetricContextRegistryBenchmark.ResolveCounterFromRegistry: Job-IMUXWQ(Runtime=.NET Core 3.1)
  DefaultMetricContextRegistryBenchmark.ResolveGaugeFromRegistry: Job-IMUXWQ(Runtime=.NET Core 3.1)
  DefaultMetricContextRegistryBenchmark.ResolveHistogramFromRegistry: Job-IMUXWQ(Runtime=.NET Core 3.1)
  DefaultMetricContextRegistryBenchmark.ResolveMeterFromRegistry: Job-IMUXWQ(Runtime=.NET Core 3.1)
  DefaultMetricContextRegistryBenchmark.ResolveTimerFromRegistry: Job-IMUXWQ(Runtime=.NET Core 3.1)
