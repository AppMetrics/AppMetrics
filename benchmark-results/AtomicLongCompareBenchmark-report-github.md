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
               Method |        Mean |      StdErr |      StdDev |      Median | Scaled | Scaled-StdDev |
--------------------- |------------ |------------ |------------ |------------ |------- |-------------- |
           AtomicLong | 813.7549 us | 122.3587 us | 273.6024 us | 697.9010 us |   1.00 |          0.00 |
     PaddedAtomicLong | 789.9080 us |  35.1259 us |  78.5438 us | 765.8195 us |   1.04 |          0.26 |
     StripedLongAdder | 834.0399 us | 117.6770 us | 263.1339 us | 787.5535 us |   1.10 |          0.41 |
 ThreadLocalLongAdder | 856.6191 us | 135.2074 us | 302.3329 us | 755.8581 us |   1.13 |          0.45 |
tegy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
  AtomicLongCompareBenchmark.StripedLongAdder: FastAndDirtyJob(Jit=RyuJit, Platform=X64, Runtime=Core, LaunchCount=1, RunStrategy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
  AtomicLongCompareBenchmark.ThreadLocalLongAdder: FastAndDirtyJob(Jit=RyuJit, Platform=X64, Runtime=Core, LaunchCount=1, RunStrategy=ColdStart, TargetCount=5, UnrollFactor=1, WarmupCount=5)
