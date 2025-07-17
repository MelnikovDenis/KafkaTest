using KafkaTest.App.Jobs;
using KafkaTest.App.MyProducer;
using KafkaTest.App.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaTest.App;

public static class DependencyInjection
{
    public static IServiceCollection AddMyProducer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetRequiredSection(nameof(KafkaOptions)));

        services.AddSingleton<MyProducerService>();
        
        services.Configure<MyProducerJobOptions>(configuration.GetSection(nameof(MyProducerJobOptions)));
        services.AddHostedService<MyProducerJob>();

        return services;
    }

    public static IServiceCollection AddMyConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetRequiredSection(nameof(KafkaOptions)));

        services.Configure<MyConsumerJobOptions>(configuration.GetSection(nameof(MyConsumerJobOptions)));
        services.AddHostedService<MyConsumerJob>();

        return services;
    }
}