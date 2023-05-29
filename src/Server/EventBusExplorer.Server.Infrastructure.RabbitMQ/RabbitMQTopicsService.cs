using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class RabbitMQTopicsService : IServiceBrokerTopicsService
{
    public RabbitMQTopicsService()
    {
    }

    public Task<string> CreateTopicAsync(string? name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetTopicAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTopicAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IList<string>> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> CreateSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
