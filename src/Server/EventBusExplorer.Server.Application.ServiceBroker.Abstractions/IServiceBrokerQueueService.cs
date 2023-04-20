namespace EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;

public interface IServiceBrokerQueueService
{
    Task<CreateQueueResponse> CreateAsync(string? name, CancellationToken cancellationToken = default);

    Task<GetQueuesResponse> GetAsync(CancellationToken cancellationToken = default);

    Task<GetQueueResponse> GetAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);
}
