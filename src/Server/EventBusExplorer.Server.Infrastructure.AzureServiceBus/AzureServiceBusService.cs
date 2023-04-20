using Azure.Messaging.ServiceBus.Administration;
using AppAbs = EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;
using Azure;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class AzureServiceBusService : AppAbs.IServiceBrokerQueueService
{
    private readonly ServiceBusAdministrationClient _adminClient;

    public AzureServiceBusService(
        ServiceBusAdministrationClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<AppAbs.CreateQueueResponse> CreateAsync(string? name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.CreateQueueAsync(name, cancellationToken);
        AppAbs.CreateQueueResponse queue = new(queueProperties.Name);
        return queue;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteQueueAsync(name, cancellationToken);
    }

    public async Task<AppAbs.GetQueuesResponse> GetAsync(CancellationToken cancellationToken = default)
    {
        List<AppAbs.GetQueuesResponseItem> results = new();
        AsyncPageable<QueueProperties> queuesProperties = _adminClient.GetQueuesAsync(cancellationToken);
        await foreach (QueueProperties queueProperties in queuesProperties)
        {
            results.Add(new AppAbs.GetQueuesResponseItem(queueProperties.Name));
        }

        return new AppAbs.GetQueuesResponse(results);
    }

    public async Task<AppAbs.GetQueueResponse> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.GetQueueAsync(name, cancellationToken);
        AppAbs.GetQueueResponse queue = new(queueProperties.Name);
        return queue;
    }
}
