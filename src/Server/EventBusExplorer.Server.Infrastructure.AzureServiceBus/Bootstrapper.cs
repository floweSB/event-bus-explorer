using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

public static class Bootstrapper
{
    /// <summary>
    /// Register Azure Service Bus as event bus
    /// </summary>
    /// <param name="services">Your <see cref="IServiceCollection"/> instance</param>
    /// <param name="configuration">Your <see cref="IConfiguration"/> instance</param>
    public static void AddAzureServiceBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        SetupServiceBus(services, configuration);

        services.AddScoped<IServiceBrokerQueuesService, ServiceBusQueuesService>();
        services.AddScoped<IServiceBrokerTopicsService, ServiceBusTopicsService>();
    }

    private static void SetupServiceBus(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureServiceBusSettings>(configuration.GetSection("ConnectionStrings")!);

        services.AddAzureClients(builder =>
        {
            //Func<string, string> GetConnectionString(IServiceProvider sp, string key)
            //{
            //    var config = sp.GetRequiredService<IOptions<AzureServiceBusSettings>>();
            //    var dict = config.Value.AzureServiceBus;

            //    return (key) =>
            //    {
            //        return dict[key]!;
            //    };
            //}

            builder.AddClient<Func<string, ServiceBusClient>, ServiceBusClientOptions>((options, _, sp) =>
            {
                var config = sp.GetRequiredService<IOptions<AzureServiceBusSettings>>();
                var dict = config.Value.AzureServiceBus;

                return (key) =>
                {
                    var cs = dict[key]!;
                    return new ServiceBusClient(cs);
                };
            });

            builder.AddClient<Func<string, ServiceBusAdministrationClient>, ServiceBusClientOptions>((options, _, sp) =>
            {
                var config = sp.GetRequiredService<IOptions<AzureServiceBusSettings>>();
                var dict = config.Value.AzureServiceBus;

                return (key) =>
                {
                    var cs = dict[key]!;
                    return new ServiceBusAdministrationClient(cs);
                };
            });

            //builder.AddServiceBusAdministrationClient(connectionString);
            //builder.AddServiceBusClient(connectionString);
        });
    }
}

public class AzureServiceBusSettings
{
    public Dictionary<string, string> AzureServiceBus { get; init; } = new Dictionary<string, string>();
}
