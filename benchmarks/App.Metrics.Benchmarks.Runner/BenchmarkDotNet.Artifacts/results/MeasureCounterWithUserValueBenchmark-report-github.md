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
------------------- |------------ |---------- |------- |
 DecrementUserValue | 312.5206 ns | 1.0196 ns | 0.0481 |
 IncrementUserValue | 312.1910 ns | 2.5753 ns | 0.0489 |
ns | 619.7298 ns | 0.0367 |
 IncrementUserValue | 640.4611 ns |  4.6083 ns |  17.8479 ns | 637.1096 ns | 0.0373 |
