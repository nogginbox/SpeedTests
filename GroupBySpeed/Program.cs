using BenchmarkDotNet.Running;
using GroupBySpeed;

var summary = BenchmarkRunner.Run<GroupByPerformanceBenchmark>();