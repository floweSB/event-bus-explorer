using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddAzureClients(builder =>
        {
            string connectionString = configuration.GetConnectionString("ServiceBus")!;
            builder.AddServiceBusAdministrationClient(connectionString);
            builder.AddServiceBusClient(connectionString);
        });
    }
}
