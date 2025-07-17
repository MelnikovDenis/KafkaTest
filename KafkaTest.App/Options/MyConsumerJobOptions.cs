namespace KafkaTest.App.Options;

internal class MyConsumerJobOptions
{
    public int Delay { get; init; } = 8;

    public TimeSpan DelayTimeSpan => TimeSpan.FromSeconds(Delay);
}
