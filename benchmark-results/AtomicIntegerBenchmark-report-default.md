
Host Process Environment Information:
BenchmarkDotNet.Core=v0.9.9.0
OS=Windows
Processor=?, ProcessorCount=8
Frequency=3312791 ticks, Resolution=301.8603 ns, Timer=TSC
CLR=CORE, Arch=64-bit ? [RyuJIT]
GC=Concurrent Workstation
dotnet cli version: 1.0.0-preview2-1-003177

Type=AtomicIntegerBenchmark  Mode=Throughput  

    Method |    Median |    StdDev |
---------- |---------- |---------- |
 Decrement | 4.0494 ns | 0.2016 ns |
       Get | 0.0000 ns | 0.0150 ns |
 Increment | 4.0331 ns | 0.5085 ns |
tomicIntegerBenchmark_Decrement
  AtomicIntegerBenchmark_Get
  AtomicIntegerBenchmark_Increment
