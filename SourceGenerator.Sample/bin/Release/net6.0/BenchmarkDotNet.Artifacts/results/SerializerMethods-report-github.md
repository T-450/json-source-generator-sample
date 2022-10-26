``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19044.2130/21H2/November2021Update)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.402
  [Host]     : .NET 6.0.10 (6.0.1022.47605), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.10 (6.0.1022.47605), X64 RyuJIT AVX2


```
|            Method |     Mean |   Error |  StdDev | Allocated |
|------------------ |---------:|--------:|--------:|----------:|
| SerializerDefault | 454.1 μs | 9.08 μs | 8.05 μs |     344 B |
|  SerializerCustom | 322.8 μs | 6.04 μs | 5.94 μs |         - |
