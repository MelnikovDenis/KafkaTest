namespace KafkaTest.App.Options;

internal class MyProducerJobOptions
{
    public int Delay { get; init; } = 5;

    public TimeSpan DelayTimeSpan => TimeSpan.FromSeconds(Delay);
}
