using EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

public static class Bootstrapper
{
    public static void AddAzureServiceBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        SetupServiceBus(services, configuration);

        services.AddScoped<IServiceBrokerQueueService, AzureServiceBusService>();
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
