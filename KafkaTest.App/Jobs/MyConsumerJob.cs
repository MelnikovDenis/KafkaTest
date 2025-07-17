using Confluent.Kafka;
using KafkaTest.App.Models;
using KafkaTest.App.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KafkaTest.App.Jobs;

internal class MyConsumerJob : BackgroundService
{
    private readonly KafkaOptions _kafkaOptions;

    private readonly MyConsumerJobOptions _jobOptions;

    private IConsumer<Null, TestModel> _consumer;

    private readonly ConsumerConfig _config;

    private readonly ILogger<MyConsumerJob> _logger;

    public MyConsumerJob(IOptions<KafkaOptions> kafkaOptions, ILogger<MyConsumerJob> logger, IOptions<MyConsumerJobOptions> jobOptions)
    {
        _kafkaOptions = kafkaOptions.Value;
        _jobOptions = jobOptions.Value;
        _logger = logger;

        _config = new ConsumerConfig()
        {
            BootstrapServers = string.Join(',', _kafkaOptions.BootstrapServers),
            GroupId = _kafkaOptions.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
        };

        _consumer = new ConsumerBuilder<Null, TestModel>(_config)
            .SetValueDeserializer(new Kafka.Json.JsonDeserializer<TestModel>())
            .Build();

        _consumer.Subscribe(_kafkaOptions.Topic);
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Бэкграунд сервис {MyConsumerJob} начал работу...", nameof(MyConsumerJob));

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Ожидание нового сообщения...");

                var consumeResult = _consumer.Consume(stoppingToken);

                _logger.LogDebug("Прочитано сообщение: {message}", consumeResult.Message.Value);

                _consumer.Commit(consumeResult);

                await Task.Delay(_jobOptions.DelayTimeSpan, stoppingToken);
            }
        }
        finally
        {
            _consumer.Unsubscribe();
            _consumer.Close();
        }
    }

    #region dispose

    ~MyConsumerJob()
    {
        Dispose(false);
    }

    private bool _isDisposed;

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        base.Dispose();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            if (disposing)
            {
                _consumer?.Dispose();
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                _consumer = null;
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            }
        }
    }

    #endregion
}