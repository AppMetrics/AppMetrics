``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
    Method |       Mean |    StdErr |    StdDev |  Gen 0 | Allocated |
---------- |----------- |---------- |---------- |------- |---------- |
 Decrement | 18.0079 ns | 0.0651 ns | 0.2523 ns |      - |       0 B |
       Get | 45.0593 ns | 0.4521 ns | 1.9181 ns | 0.0174 |      79 B |
 Increment | 17.7471 ns | 0.0907 ns | 0.3514 ns |      - |       0 B |
