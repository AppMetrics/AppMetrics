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
 Decrement | 38.0344 us | 37.2822 us | 83.3656 us | 0.3019 us |
       Get | 39.3626 us | 38.6845 us | 86.5012 us | 0.3019 us |
 Increment | 38.2759 us | 37.6735 us | 84.2405 us | 0.3019 us |
s |
