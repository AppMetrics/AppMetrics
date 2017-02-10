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
                        TimeAlgorithmR | 6.1533 us | 0.0185 us |     327 B |
      TrackUsingAlgorithmRUsingContext | 6.1301 us | 0.0233 us |     327 B |
             TrackUsingForwardDecaying | 6.1095 us | 0.0646 us |     391 B |
 TrackUsingForwardDecayingUsingContext | 6.4140 us | 0.0470 us |     327 B |
               TrackUsingSlidingWindow | 6.1876 us | 0.0458 us |     327 B |
   TrackUsingSlidingWindowUsingContext | 6.0729 us | 0.0764 us |     327 B |
