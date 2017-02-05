``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
         Method |        Mean |     StdDev |  Gen 0 | Allocated |
--------------- |------------ |----------- |------- |---------- |
           Mark | 199.1586 ns |  1.6771 ns | 0.0484 |     255 B |
 MarkMetricItem | 546.7033 ns | 14.4922 ns | 0.0631 |     366 B |
  MarkUserValue | 350.2237 ns |  4.4682 ns | 0.0490 |     255 B |
 255 B |
