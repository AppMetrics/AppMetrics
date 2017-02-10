``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=287 B  

```
                Method |        Mean |    StdDev |  Gen 0 |
---------------------- |------------ |---------- |------- |
      UpdateAlgorithmR | 360.2939 ns | 1.4336 ns | 0.0434 |
 UpdateForwardDecaying | 527.5828 ns | 3.1648 ns | 0.0429 |
   UpdateSlidingWindow | 320.9964 ns | 1.4441 ns | 0.0552 |
