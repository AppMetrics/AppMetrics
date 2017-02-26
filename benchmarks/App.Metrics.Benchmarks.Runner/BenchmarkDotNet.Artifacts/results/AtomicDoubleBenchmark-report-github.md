``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=0 B  

```
    Method |       Mean |    StdErr |    StdDev |
---------- |----------- |---------- |---------- |
 Decrement | 11.3456 ns | 0.0239 ns | 0.0927 ns |
       Get |  0.0019 ns | 0.0008 ns | 0.0033 ns |
 Increment | 11.4161 ns | 0.0348 ns | 0.1348 ns |
