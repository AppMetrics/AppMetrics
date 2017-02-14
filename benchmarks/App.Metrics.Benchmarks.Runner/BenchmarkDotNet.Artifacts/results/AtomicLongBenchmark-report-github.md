``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=0 B  

```
    Method |      Mean |    StdErr |    StdDev |
---------- |---------- |---------- |---------- |
 Decrement | 4.7827 ns | 0.0155 ns | 0.0601 ns |
       Get | 0.0018 ns | 0.0014 ns | 0.0052 ns |
 Increment | 4.7886 ns | 0.0115 ns | 0.0447 ns |
