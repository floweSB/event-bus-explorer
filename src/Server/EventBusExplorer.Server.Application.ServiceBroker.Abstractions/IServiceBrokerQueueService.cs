namespace EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;

public interface IServiceBrokerQueueService
{
    Task<Queue> CreateAsync(string name, CancellationToken cancellationToken = default);

    Task<IList<Queue>> GetAsync(CancellationToken cancellationToken = default);

    Task<Queue> GetAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);
}

public class Queue
{
    public string Name { get; }
    public Queue(string name)
    {
        Name = name;
    }
}
