``` ini

BenchmarkDotNet=v0.10.1, OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312782 Hz, Resolution=301.8611 ns, Timer=TSC
dotnet cli version=1.0.0-preview2-1-003177
  [Host] : .NET Core 4.6.24628.01, 64bit RyuJIT
  Core   : .NET Core 4.6.24628.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
                                Method |      Mean |    StdDev | Allocated |
-------------------------------------- |---------- |---------- |---------- |
                        TimeAlgorithmR | 6.3008 us | 0.2169 us |     327 B |
      TrackUsingAlgorithmRUsingContext | 6.2410 us | 0.1050 us |     327 B |
             TrackUsingForwardDecaying | 6.0495 us | 0.0328 us |     391 B |
 TrackUsingForwardDecayingUsingContext | 6.3460 us | 0.0579 us |     327 B |
               TrackUsingSlidingWindow | 6.0614 us | 0.0171 us |     327 B |
   TrackUsingSlidingWindowUsingContext | 6.0760 us | 0.0425 us |     327 B |
