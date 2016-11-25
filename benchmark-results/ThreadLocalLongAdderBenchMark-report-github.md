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
 Decrement | 41.2341 us | 40.4054 us | 90.3492 us | 0.6037 us |
       Get | 39.3022 us | 37.1156 us | 82.9930 us | 1.8112 us |
 Increment | 37.6118 us | 36.8588 us | 82.4188 us | 0.3019 us |
