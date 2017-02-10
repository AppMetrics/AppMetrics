``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=255 B  

```
    Method |        Mean |    StdDev |  Gen 0 |
---------- |------------ |---------- |------- |
 Decrement | 217.1272 ns | 2.7000 ns | 0.0484 |
 Increment | 220.4017 ns | 4.3155 ns | 0.0476 |
