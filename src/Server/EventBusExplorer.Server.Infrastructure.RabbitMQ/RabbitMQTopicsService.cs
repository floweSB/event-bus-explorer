using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public class RabbitMQTopicsService : IServiceBrokerTopicsService
{
    private readonly RabbitMQAdministrationClient _adminClient;

    public RabbitMQTopicsService(
        RabbitMQAdministrationClient adminClient
    )
    {
        _adminClient = adminClient;
    }

    public async Task<string> CreateTopicAsync(string? name, CancellationToken cancellationToken = default)
    {
        ExchangeTopic topic = await _adminClient.CreateTopicAsync(
            name,
            cancellationToken: cancellationToken);

        return topic.Name;
    }

    public async Task<IList<string>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        IList<ExchangeTopic> topics = await _adminClient.GetTopicsAsync(
            cancellationToken: cancellationToken);

        return topics.Select(x => x.Name).ToList();
    }

    public async Task<string> GetTopicAsync(string name, CancellationToken cancellationToken = default)
    {
        ExchangeTopic topic = await _adminClient.GetTopicAsync(
            name,
            cancellationToken: cancellationToken);

        return topic.Name;
    }

    public async Task DeleteTopicAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteTopicAsync(name, cancellationToken: cancellationToken);
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
