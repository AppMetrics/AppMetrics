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
 Decrement | 36.7062 us | 36.0299 us |  80.5652 us | 0.3019 us |
       Get | 45.0979 us | 44.4959 us |  99.4960 us | 0.3019 us |
 Increment | 55.1801 us | 54.3529 us | 121.5368 us | 0.3019 us |
