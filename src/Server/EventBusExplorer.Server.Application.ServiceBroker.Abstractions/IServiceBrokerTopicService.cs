namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

public interface IServiceBrokerTopicsService
{
    Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default);

    Task<IList<string>> GetAsync(CancellationToken cancellationToken = default);

    Task<string> GetAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteAsync(string name, CancellationToken cancellationToken = default);

    Task<IList<string>> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default);

    Task<string> GetSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);

    Task<string> CreateSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default);

    Task DeleteSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);
}
