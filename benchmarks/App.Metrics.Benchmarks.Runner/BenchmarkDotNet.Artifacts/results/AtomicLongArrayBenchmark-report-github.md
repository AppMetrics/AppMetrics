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
 Decrement | 4.8550 ns | 0.0182 ns | 0.0681 ns |
       Get | 0.0724 ns | 0.0039 ns | 0.0142 ns |
 Increment | 4.7612 ns | 0.0133 ns | 0.0478 ns |
