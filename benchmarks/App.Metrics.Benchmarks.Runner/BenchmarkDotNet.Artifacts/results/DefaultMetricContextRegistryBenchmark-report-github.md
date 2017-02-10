``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=191 B  

```
                       Method |       Mean |    StdDev |  Gen 0 |
----------------------------- |----------- |---------- |------- |
     ResolveApdexFromRegistry | 77.6779 ns | 0.6714 ns | 0.0423 |
   ResolveCounterFromRegistry | 78.5300 ns | 0.4172 ns | 0.0423 |
     ResolveGaugeFromRegistry | 76.2272 ns | 1.6039 ns | 0.0425 |
 ResolveHistogramFromRegistry | 79.3706 ns | 0.5326 ns | 0.0426 |
     ResolveMeterFromRegistry | 76.2006 ns | 0.4413 ns | 0.0422 |
     ResolveTimerFromRegistry | 77.0501 ns | 0.9058 ns | 0.0424 |
