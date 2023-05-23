namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

public interface IServiceBrokerTopicsService
{
    Task<string> CreateTopicAsync(string? name, CancellationToken cancellationToken = default);

    Task<IList<string>> GetTopicsAsync(CancellationToken cancellationToken = default);

    Task<string> GetTopicAsync(string name, CancellationToken cancellationToken = default);

    Task DeleteTopicAsync(string name, CancellationToken cancellationToken = default);

    Task<IList<string>> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default);

    Task<string> GetSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);

    Task<string> CreateSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default);

    Task DeleteSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);
}
