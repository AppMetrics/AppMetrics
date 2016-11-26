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
 Decrement | 39.0003 us | 38.3238 us |  85.6947 us | 0.3019 us |
       Get | 56.5082 us | 55.8309 us | 124.8416 us | 0.3019 us |
 Increment | 37.4910 us | 36.7399 us |  82.1530 us | 0.3019 us |
