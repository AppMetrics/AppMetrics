``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
               Method |        Mean |     StdDev | Scaled | Scaled-StdDev |   Gen 0 | Allocated |
--------------------- |------------ |----------- |------- |-------------- |-------- |---------- |
           AtomicLong | 501.5930 us | 17.4125 us |   1.00 |          0.00 |       - |   1.79 kB |
     PaddedAtomicLong | 492.6155 us |  8.4985 us |   0.98 |          0.04 |       - |   2.03 kB |
     StripedLongAdder | 494.9937 us |  5.3520 us |   0.99 |          0.03 |       - |    1.8 kB |
 ThreadLocalLongAdder | 579.8045 us |  8.4721 us |   1.16 |          0.04 | 21.3542 |   2.27 kB |
