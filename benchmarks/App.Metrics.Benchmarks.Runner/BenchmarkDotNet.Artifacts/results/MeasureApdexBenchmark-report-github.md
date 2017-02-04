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
                        TimeAlgorithmR | 6.3964 us | 0.0334 us |     327 B |
      TrackUsingAlgorithmRUsingContext | 6.4500 us | 0.1157 us |     327 B |
             TrackUsingForwardDecaying | 6.2112 us | 0.0674 us |     391 B |
 TrackUsingForwardDecayingUsingContext | 6.5129 us | 0.2295 us |     327 B |
               TrackUsingSlidingWindow | 6.1963 us | 0.0361 us |     327 B |
   TrackUsingSlidingWindowUsingContext | 6.2162 us | 0.0568 us |     327 B |
