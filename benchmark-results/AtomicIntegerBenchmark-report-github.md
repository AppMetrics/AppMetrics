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

Job=QuickRun  Jit=RyuJit  Platform=X64  
Runtime=Core  LaunchCount=1  RunStrategy=ColdStart  
TargetCount=5  UnrollFactor=1  WarmupCount=5  

```
    Method |       Mean |     StdErr |      StdDev |    Median |
---------- |----------- |----------- |------------ |---------- |
 Decrement | 56.3875 us | 55.7847 us | 124.7383 us | 0.3019 us |
       Get | 38.2155 us | 37.6888 us |  84.2746 us | 0.3019 us |
 Increment | 48.2976 us | 47.5457 us | 106.3155 us | 0.3019 us |
re, LaunchCount=1, RunStrategy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
  AtomicIntegerBenchmark.Increment: QuickRun(Jit=RyuJit, Platform=X64, Runtime=Core, LaunchCount=1, RunStrategy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
