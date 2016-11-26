``` ini

BenchmarkDotNet=v0.10.0
OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312791 Hz, Resolution=301.8603 ns, Timer=TSC
Host Runtime=.NET Core 4.6.24410.01, Arch=64-bit  [RyuJIT]
GC=Concurrent Workstation
dotnet cli version=1.0.0-preview2-1-003177
Job Runtime(s):
	.NET Core 4.6.24410.01, Arch=64-bit  [RyuJIT]

Job=MediumRun  Jit=RyuJit  Platform=X64  
Runtime=Core  LaunchCount=2  TargetCount=15  
WarmupCount=10  

```
                         Method |          Mean |     StdErr |      StdDev |        Median | Scaled | Scaled-StdDev |
------------------------------- |-------------- |----------- |------------ |-------------- |------- |-------------- |
               UniformReservoir |   717.8625 us |  4.4517 us |  23.1318 us |   706.8204 us |   1.00 |          0.00 |
         SlidingWindowReservoir |   717.1000 us |  5.0570 us |  27.2327 us |   702.2431 us |   1.00 |          0.05 |
 ExponentiallyDecayingReservoir | 8,931.7810 us | 91.0223 us | 498.5499 us | 8,912.4664 us |  12.45 |          0.78 |
          HdrHistogramReservoir | 7,797.4069 us | 88.6230 us | 485.4081 us | 7,667.7400 us |  10.87 |          0.74 |
