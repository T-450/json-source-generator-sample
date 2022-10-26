using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

BenchmarkRunner.Run(typeof(SerializerMethods).Assembly);

[MemoryDiagnoser]
public class SerializerMethods
{
    private MemoryStream? _memoryStream;
    private Utf8JsonWriter? _jsonWriter;
    private Person[]? _people;

    [GlobalSetup]
    public void Setup()
    {
        _memoryStream = new MemoryStream();
        _jsonWriter = new Utf8JsonWriter(_memoryStream);

        _people = DataFakePeople.GetPeople();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _memoryStream?.Dispose();
        _jsonWriter?.Dispose();
    }
     
    [Benchmark]
    public void SerializerDefault()
    {
        JsonSerializer.Serialize(_jsonWriter!, _people);

        _memoryStream!.SetLength(0);
        _jsonWriter!.Reset();
    }

    [Benchmark]
    public void SerializerCustom()
    {
        JsonSerializer.Serialize(_jsonWriter, _people, PersonCustomContext.Default.PersonArray);

        _memoryStream!.SetLength(0);
        _jsonWriter!.Reset();
    }
}

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(Person[]))]
internal partial class PersonCustomContext : JsonSerializerContext
{

}

internal static class DataFakePeople
{
    public static Person[] GetPeople()
    {
        return Enumerable.Range(1, 1000).Select(i =>
            {
                return new Faker<Person>("pt_BR")
                    .RuleFor(c => c.Id, f => f.Random.Guid())
                    .RuleFor(c => c.NomeCompleto, f => f.Name.FullName())
                    .RuleFor(c => c.DataNascimento, f => f.Date.Past(20))
                    .Generate();
            }
        ).ToArray();
    }
}

public record Person
{
    public Guid Id { get; set; }
    public string? NomeCompleto { get; set; }
    public DateTime DataNascimento { get; set; }
}