namespace KafkaTest.App.Options;

internal class KafkaOptions
{
    public IReadOnlyList<string> BootstrapServers { get; init; } = [];

    public required string Topic { get; init; }
}
