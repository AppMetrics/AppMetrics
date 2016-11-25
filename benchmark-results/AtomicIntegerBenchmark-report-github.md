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

Job=FastAndDirtyJob  Jit=RyuJit  Platform=X64  
Runtime=Core  LaunchCount=1  RunStrategy=ColdStart  
TargetCount=5  UnrollFactor=1  WarmupCount=5  

```
    Method |       Mean |     StdErr |     StdDev |    Median |
---------- |----------- |----------- |----------- |---------- |
 Decrement | 38.8796 us | 38.1274 us | 85.2555 us | 0.3019 us |
       Get | 37.5514 us | 36.9491 us | 82.6206 us | 0.3019 us |
 Increment | 40.9926 us | 40.5414 us | 90.6532 us | 0.0000 us |
s |
Factor=1, WarmupCount=1)
  AtomicIntegerBenchmark.Get: MySuperJob(MaxStdErrRelative=0.01, Jit=RyuJit, Platform=X64, Runtime=Core, IterationTime=200.0000 ms, LaunchCount=1, RunStrategy=ColdStart, TargetCount=1, UnrollFactor=1, WarmupCount=1)
  AtomicIntegerBenchmark.Increment: MySuperJob(MaxStdErrRelative=0.01, Jit=RyuJit, Platform=X64, Runtime=Core, IterationTime=200.0000 ms, LaunchCount=1, RunStrategy=ColdStart, TargetCount=1, UnrollFactor=1, WarmupCount=1)
