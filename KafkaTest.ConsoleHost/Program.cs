using KafkaTest.App;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureHostConfiguration(config => 
{
    config.AddJsonFile("appsettings.json", false, false);
});

builder.ConfigureLogging((context, logging) =>
{
    logging.AddConfiguration(context.Configuration);
    logging.AddConsole();
});

builder.ConfigureServices((context, services) =>
{
    services.AddMyServices(context.Configuration);
});

var host = builder.Build();

host.Run();