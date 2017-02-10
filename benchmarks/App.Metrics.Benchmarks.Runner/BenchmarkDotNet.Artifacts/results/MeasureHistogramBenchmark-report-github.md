``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=287 B  

```
                Method |        Mean |     StdDev |  Gen 0 |
---------------------- |------------ |----------- |------- |
      UpdateAlgorithmR | 378.4413 ns | 15.4803 ns | 0.0555 |
 UpdateForwardDecaying | 548.6866 ns |  6.6498 ns | 0.0439 |
   UpdateSlidingWindow | 281.3492 ns |  2.3734 ns | 0.0565 |
   NA |         NA |    N/A |       N/A |

Benchmarks with issues:
  MeasureHistogramBenchmark.UpdateForwardDecaying: Core(Runtime=Core)
  MeasureHistogramBenchmark.UpdateSlidingWindow: Core(Runtime=Core)
