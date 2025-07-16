using Confluent.Kafka;
using KafkaTest.App.Models;
using KafkaTest.App.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KafkaTest.App.MyProducer;

internal class MyProducerService : IDisposable
{
    private readonly KafkaOptions _options;

    private readonly ProducerConfig _config;

    private IProducer<Null, TestModel> _producer;

    private readonly ILogger<MyProducerService> _logger;

    public MyProducerService(IOptions<KafkaOptions> options, ILogger<MyProducerService> logger)
    {
        _options = options.Value;

        _config = new ProducerConfig()
        {
            BootstrapServers = string.Join(',', _options.BootstrapServers),
            Acks = Acks.All
        };

        _producer = new ProducerBuilder<Null, TestModel>(_config)
            .SetValueSerializer(new KafkaTest.App.Kafka.Json.JsonSerializer<TestModel>())
            .Build();

        _logger = logger;
    }

    public async Task ProduceAsync(TestModel testModel, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Продьюсинг сообщения: {testModel}...", testModel);

        await _producer.ProduceAsync(_options.Topic, new Message<Null, TestModel> { Value = testModel }, cancellationToken);

        _logger.LogDebug("Продьюсинг сообщения: {testModel}. Успешно.", testModel);
    }

    #region dispose

    ~MyProducerService()
    {
        Dispose(false);
    }

    private bool _isDisposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            _isDisposed = true;

            if (disposing)
            {
                _producer.Dispose();
#pragma warning disable CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
                _producer = null;
#pragma warning restore CS8625 // Литерал, равный NULL, не может быть преобразован в ссылочный тип, не допускающий значение NULL.
            }
        }
    }

    #endregion
}

