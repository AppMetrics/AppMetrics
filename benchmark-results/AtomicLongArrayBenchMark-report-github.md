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
    Method |       Mean |     StdErr |     StdDev |    Median |
---------- |----------- |----------- |----------- |---------- |
 Decrement | 39.6644 us | 39.0627 us | 87.3469 us | 0.3019 us |
       Get | 37.4307 us | 36.7527 us | 82.1814 us | 0.3019 us |
 Increment | 36.1629 us | 35.4849 us | 79.3466 us | 0.3019 us |
untime=Core, LaunchCount=1, RunStrategy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
  AtomicLongArrayBenchmark.Increment: QuickRun(Jit=RyuJit, Platform=X64, Runtime=Core, LaunchCount=1, RunStrategy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
llFactor=1, WarmupCount=5)
