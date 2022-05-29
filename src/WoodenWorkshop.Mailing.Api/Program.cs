using RabbitMQ.Client;

using WoodenWorkshop.Mailing.Api;
using WoodenWorkshop.Mailing.Core;
using WoodenWorkshop.Mailing.Core.Options;

var configuration = BuildConfiguration();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => config.AddConfiguration(configuration))
    .ConfigureServices(services =>
    {
        var messageQueueConfiguration = configuration.GetSection("MessageQueue");
        services.AddSingleton<IConnectionFactory>(
            _ => new ConnectionFactory
            {
                HostName = messageQueueConfiguration.GetValue<string>("Host"),
                UserName = messageQueueConfiguration.GetValue<string>("Username"),
                Password = messageQueueConfiguration.GetValue<string>("Password"),
            }
        );

        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
        services.AddScoped<Mailer>();
        services.AddHostedService<EmailQueueWorker>();
    })
    .Build();

await host.RunAsync();

IConfiguration BuildConfiguration()
{
    var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    var builder = new ConfigurationBuilder();
    builder.AddEnvironmentVariables()
        .AddJsonFile("appsettings.json", true)
        .AddJsonFile($"appsettings.{environmentName}.json", true);

    var azureAppConfigConnectionString = Environment.GetEnvironmentVariable("AppConfigurationConnectionString");
    if (!string.IsNullOrEmpty(azureAppConfigConnectionString))
    {
        builder.AddAzureAppConfiguration(azureAppConfigConnectionString);
    }

    return builder.Build();
}
