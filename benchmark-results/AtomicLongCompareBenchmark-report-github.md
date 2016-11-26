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
               Method |        Mean |     StdDev |      Median | Scaled | Scaled-StdDev |
--------------------- |------------ |----------- |------------ |------- |-------------- |
           AtomicLong | 510.2258 us | 19.0166 us | 506.6581 us |   1.00 |          0.00 |
     PaddedAtomicLong | 500.9378 us | 14.0043 us | 495.6003 us |   0.98 |          0.04 |
     StripedLongAdder | 515.2295 us | 19.3471 us | 513.6489 us |   1.01 |          0.05 |
 ThreadLocalLongAdder | 660.2230 us | 32.1572 us | 653.3072 us |   1.30 |          0.08 |
