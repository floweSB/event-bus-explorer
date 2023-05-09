using Azure;
using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class ServiceBusQueuesService : IServiceBrokerQueuesService
{
    private readonly ServiceBusAdministrationClient _adminClient;

    public ServiceBusQueuesService(
        ServiceBusAdministrationClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.CreateQueueAsync(name, cancellationToken);
        return queueProperties.Name;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteQueueAsync(name, cancellationToken);
    }

    public async Task<IList<string>> GetAsync(CancellationToken cancellationToken = default)
    {
        List<string> queues = new();
        AsyncPageable<QueueProperties> queuesProperties = _adminClient.GetQueuesAsync(cancellationToken);
        await foreach (QueueProperties queueProperties in queuesProperties)
        {
            queues.Add(queueProperties.Name);
        }

        return queues;
    }

    public async Task<string> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.GetQueueAsync(name, cancellationToken);
        return queueProperties.Name;
    }
}
