``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=191 B  

```
                       Method |       Mean |    StdErr |    StdDev |  Gen 0 |
----------------------------- |----------- |---------- |---------- |------- |
     ResolveApdexFromRegistry | 77.9102 ns | 0.7275 ns | 2.7220 ns | 0.0424 |
   ResolveCounterFromRegistry | 78.5771 ns | 1.0873 ns | 4.2112 ns | 0.0436 |
     ResolveGaugeFromRegistry | 75.2492 ns | 0.2946 ns | 1.1410 ns | 0.0424 |
 ResolveHistogramFromRegistry | 81.3198 ns | 0.2382 ns | 0.8913 ns | 0.0431 |
     ResolveMeterFromRegistry | 77.3595 ns | 0.1044 ns | 0.4044 ns | 0.0425 |
     ResolveTimerFromRegistry | 74.9087 ns | 0.0729 ns | 0.2728 ns | 0.0423 |
