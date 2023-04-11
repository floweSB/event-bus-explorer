namespace EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;

public interface IServiceBrokerQueueService
{
    Task<Queue> CreateAsync(string name);

    Task<IList<Queue>> GetAsync();

    Task<Queue> GetAsync(string name);

    Task DeleteAsync(string name);
}

public class Queue
{
    public string Name { get; }
    public Queue(string name)
    {
        Name = name;
    }
}
