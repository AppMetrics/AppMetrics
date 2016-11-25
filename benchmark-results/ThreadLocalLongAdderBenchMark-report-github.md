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
 Decrement | 39.3022 us | 38.3224 us |  85.6915 us | 0.9056 us |
       Get | 42.2604 us | 38.0043 us |  84.9802 us | 2.4149 us |
 Increment | 47.3921 us | 46.3369 us | 103.6124 us | 0.6037 us |
