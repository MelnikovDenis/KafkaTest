using KafkaTest.App.Models;
using KafkaTest.App.MyProducer;
using KafkaTest.App.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KafkaTest.App.Jobs;

internal class MyProducerJob(MyProducerService producer, 
    IOptions<MyProducerJobOptions> options, 
    ILogger<MyProducerJob> logger) : BackgroundService
{
    private readonly MyProducerService _producer = producer;

    private readonly MyProducerJobOptions _options = options.Value;

    private readonly ILogger<MyProducerJob> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Бэкграунд сервис {myProducerJob} начал работу...", nameof(MyProducerJob));

        while (!stoppingToken.IsCancellationRequested)
        {
            await _producer.ProduceAsync(new TestModel(), stoppingToken);

            await Task.Delay(_options.DelayTimeSpan, stoppingToken);
        }
    }
}
