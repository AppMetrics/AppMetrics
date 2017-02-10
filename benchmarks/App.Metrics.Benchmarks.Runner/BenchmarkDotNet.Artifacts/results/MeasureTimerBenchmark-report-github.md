``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=327 B  

```
                          Method |      Mean |    StdDev |
-------------------------------- |---------- |---------- |
                  TimeAlgorithmR | 6.4408 us | 0.0286 us |
      TimeAlgorithmRUsingContext | 6.0420 us | 0.0420 us |
             TimeForwardDecaying | 6.5738 us | 0.0419 us |
 TimeForwardDecayingUsingContext | 6.1245 us | 0.0377 us |
               TimeSlidingWindow | 6.1569 us | 0.0345 us |
   TimeSlidingWindowUsingContext | 5.8568 us | 0.0208 us |
