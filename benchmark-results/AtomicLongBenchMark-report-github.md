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
 Decrement | 37.9137 us | 37.3863 us | 83.5983 us | 0.3019 us |
       Get | 37.1288 us | 36.5265 us | 81.6757 us | 0.3019 us |
 Increment | 38.8192 us | 36.2958 us | 81.1599 us | 0.3019 us |
s |
