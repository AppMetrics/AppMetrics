``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  Allocated=327 B  

```
                          Method |      Mean |    StdErr |    StdDev |
-------------------------------- |---------- |---------- |---------- |
                  TimeAlgorithmR | 6.4128 us | 0.0430 us | 0.1665 us |
      TimeAlgorithmRUsingContext | 6.3577 us | 0.0172 us | 0.0667 us |
             TimeForwardDecaying | 6.5900 us | 0.0151 us | 0.0523 us |
 TimeForwardDecayingUsingContext | 6.4094 us | 0.0207 us | 0.0803 us |
               TimeSlidingWindow | 6.1766 us | 0.0121 us | 0.0452 us |
   TimeSlidingWindowUsingContext | 6.3266 us | 0.0690 us | 0.2844 us |
