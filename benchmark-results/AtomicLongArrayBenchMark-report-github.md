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
 Decrement | 37.8533 us | 37.2516 us | 83.2972 us | 0.3019 us |
       Get | 36.9477 us | 36.2697 us | 81.1015 us | 0.3019 us |
 Increment | 36.1025 us | 35.5018 us | 79.3845 us | 0.0000 us |
s |
