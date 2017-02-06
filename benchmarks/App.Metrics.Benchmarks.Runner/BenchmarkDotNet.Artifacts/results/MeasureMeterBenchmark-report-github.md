``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
         Method |        Mean |    StdDev |  Gen 0 | Allocated |
--------------- |------------ |---------- |------- |---------- |
           Mark | 195.2061 ns | 2.5404 ns | 0.0482 |     255 B |
 MarkMetricItem | 399.7034 ns | 1.7561 ns | 0.0615 |     366 B |
  MarkUserValue | 303.7152 ns | 3.0520 ns | 0.0482 |     255 B |
B |
 255 B |
