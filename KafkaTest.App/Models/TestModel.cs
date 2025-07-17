using System.Security.Cryptography;

namespace KafkaTest.App.Models;

public record class TestModel
{
    private static int idCounter = 0;

    private readonly static IReadOnlyList<string> SeedingAdjectives = [
        "Brave",
        "Silent",
        "Clever",
        "Bright",
        "Gentle",
        "Fierce",
        "Mighty",
        "Swift",
        "Happy",
        "Calm"
    ];

    private readonly static IReadOnlyList<string> SeedingNouns = [
        "Falcon",
        "River",
        "Mountain",
        "Shadow",
        "Lion",
        "Forest",
        "Wolf",
        "Storm",
        "Ocean",
        "Flame"
    ];

    public TestModel()
    {
        Id = Interlocked.Increment(ref idCounter);

        var adjective = SeedingAdjectives[RandomNumberGenerator.GetInt32(0, SeedingAdjectives.Count)];
        var noun = SeedingNouns[RandomNumberGenerator.GetInt32(0, SeedingNouns.Count)];

        Name = string.Join(' ', adjective, noun); 
    }

    public TestModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; init; }
}
