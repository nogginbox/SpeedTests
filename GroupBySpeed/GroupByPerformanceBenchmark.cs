using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

namespace GroupBySpeed;

[MemoryDiagnoser]
public class GroupByPerformanceBenchmark
{
    private readonly List<TestRecord> _records = [];

    [GlobalSetup]
    public void GlobalSetup()
    {
        var recordFaker = new Faker<TestRecord>()
            .StrictMode(true)
            .RuleFor(r => r.Surname, f => f.PickRandom("Garside", "Bagley", "Amesbury", "Coverdale"))
            .RuleFor(o => o.FirstName, f => $"Richard{f.Random.Number(1, 10)}");
       
        _records.AddRange(recordFaker.GenerateBetween(10_000, 10_000));
    }

    [Benchmark]
    public void TupleGrouper()
    {
        var group = _records.GroupBy(r => (r.FirstName?.ToLower(), r.Surname?.ToLower())).ToList();
        if(group.Count == 0)
        {
            throw new InvalidBenchmarkDeclarationException("Did not group stuff");
        }
    }

    [Benchmark]
    public void ConcatStringGrouper()
    {
        var group = _records.GroupBy(r => $"{r.FirstName}.{r.Surname}", StringComparer.OrdinalIgnoreCase).ToList();
        if (group.Count == 0)
        {
            throw new InvalidBenchmarkDeclarationException("Did not group stuff");
        }
    }
}
