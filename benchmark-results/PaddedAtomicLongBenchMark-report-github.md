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
    Method |       Mean |     StdErr |      StdDev |    Median |
---------- |----------- |----------- |------------ |---------- |
 Decrement | 37.4910 us | 36.8887 us |  82.4856 us | 0.3019 us |
       Get | 37.7325 us | 37.1302 us |  83.0256 us | 0.3019 us |
 Increment | 46.6072 us | 45.8546 us | 102.5340 us | 0.3019 us |
