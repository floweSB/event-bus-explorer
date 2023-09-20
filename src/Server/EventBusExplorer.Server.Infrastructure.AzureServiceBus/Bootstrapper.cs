using System.Collections.ObjectModel;
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
        services.Configure<EventBusSettings>(configuration.GetSection("ConnectionStrings:EventBus")!);

        services.AddAzureClients(builder =>
        {
            builder.AddClient<Func<string, ServiceBusClient>, ServiceBusClientOptions>((options, _, sp) =>
            {
                var config = sp.GetRequiredService<IOptions<EventBusSettings>>();
                var dict = config.Value.AzureServiceBus;

                return (key) =>
                {
                    var isFound = dict.TryGetValue(key, out string cs);
                    if (!isFound)
                    {
                        if (dict.Count == 1)
                        {
                            cs = dict.FirstOrDefault().Value;
                        }
                    }

                    return new ServiceBusClient(cs);
                };
            });

            builder.AddClient<Func<string, ServiceBusAdministrationClient>, ServiceBusClientOptions>((options, _, sp) =>
            {
                var config = sp.GetRequiredService<IOptions<EventBusSettings>>();
                var dict = config.Value.AzureServiceBus;

                return (key) =>
                {
                    var isFound = dict.TryGetValue(key, out string cs);
                    if (!isFound)
                    {
                        if (dict.Count == 1)
                        {
                            cs = dict.FirstOrDefault().Value;
                        }
                    }

                    return new ServiceBusAdministrationClient(cs);
                };
            });
        });
    }
}

public class EventBusSettings
{
    public IDictionary<string, string> AzureServiceBus { get; init; } = new Dictionary<string, string>();

    public ReadOnlyCollection<string> ServiceBusNames =>
        AzureServiceBus
            .Keys
            .OrderBy(x => x)
            .ToList()
            .AsReadOnly();
}
