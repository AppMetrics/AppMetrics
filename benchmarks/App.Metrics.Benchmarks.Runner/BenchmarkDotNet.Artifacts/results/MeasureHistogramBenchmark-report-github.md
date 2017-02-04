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
      UpdateAlgorithmR | 425.3188 ns |  4.5029 ns | 0.0429 |
 UpdateForwardDecaying | 472.2366 ns | 13.0843 ns | 0.0429 |
   UpdateSlidingWindow | 325.0733 ns |  1.5182 ns | 0.0563 |
