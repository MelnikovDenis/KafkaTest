using KafkaTest.App.Jobs;
using KafkaTest.App.MyProducer;
using KafkaTest.App.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KafkaTest.App;

public static class DependencyInjection
{
    public static IServiceCollection AddMyServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(configuration.GetRequiredSection(nameof(KafkaOptions)));

        services.AddSingleton<MyProducerService>();
        
        services.Configure<MyProducerJobOptions>(configuration.GetSection(nameof(MyProducerJobOptions)));
        services.AddHostedService<MyProducerJob>();

        return services;
    }
}