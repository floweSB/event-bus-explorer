using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;
using Azure;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class AzureServiceBusService : IServiceBrokerQueueService
{
    private readonly ServiceBusAdministrationClient _adminClient;

    public AzureServiceBusService(
        ServiceBusAdministrationClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<Queue> CreateAsync(string name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.CreateQueueAsync(name, cancellationToken);
        Queue queue = new Queue(queueProperties.Name);
        return queue;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteQueueAsync(name, cancellationToken);
    }

    public async Task<IList<Queue>> GetAsync(CancellationToken cancellationToken = default)
    {
        IList<Queue> queues = new List<Queue>();
        AsyncPageable<QueueProperties> queuesProperties = _adminClient.GetQueuesAsync(cancellationToken);
        await foreach (QueueProperties queueProperties in queuesProperties)
        {
            queues.Add(new Queue(queueProperties.Name));
        }
        return queues;
    }

    public async Task<Queue> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.GetQueueAsync(name, cancellationToken);
        Queue queue = new Queue(queueProperties.Name);
        return queue;
    }
}
