``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=0 B  

```
    Method |       Mean |    StdDev |
---------- |----------- |---------- |
 Decrement | 10.5946 ns | 0.0702 ns |
       Get |  2.0087 ns | 0.0104 ns |
 Increment | 10.5074 ns | 0.0628 ns |
