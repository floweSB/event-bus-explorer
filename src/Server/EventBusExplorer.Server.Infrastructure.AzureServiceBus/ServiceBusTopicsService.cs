using Azure;
using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class ServiceBusTopicsService : IServiceBrokerTopicsService
{
    private readonly ServiceBusAdministrationClient _adminClient;

    public ServiceBusTopicsService(
        ServiceBusAdministrationClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default)
    {
        TopicProperties topicProperties = await _adminClient.CreateTopicAsync(name, cancellationToken);
        return topicProperties.Name;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteTopicAsync(name, cancellationToken);
    }

    public async Task<IList<string>> GetAsync(CancellationToken cancellationToken = default)
    {
        List<string> topics = new();
        AsyncPageable<TopicProperties> topicsProperties = _adminClient.GetTopicsAsync(cancellationToken);
        await foreach (TopicProperties topicProperties in topicsProperties)
        {
            topics.Add(topicProperties.Name);
        }

        return topics;
    }

    public async Task<string> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        TopicProperties topicProperties = await _adminClient.GetTopicAsync(name, cancellationToken);
        return topicProperties.Name;
    }

    public async Task<IList<string>> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default)
    {
        List<string> subscriptions = new();
        AsyncPageable<SubscriptionProperties> subProperties = _adminClient.GetSubscriptionsAsync(topicName, cancellationToken);
        await foreach (SubscriptionProperties subscriptionProperties in subProperties)
        {
            subscriptions.Add(subscriptionProperties.SubscriptionName);
        }

        return subscriptions;
    }

    public async Task<string> GetSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default)
    {
        SubscriptionProperties subProperties = await _adminClient.GetSubscriptionAsync(topicName, subscriptionName, cancellationToken);
        return subProperties.SubscriptionName;
    }

    public async Task<string> CreateSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default)
    {
        SubscriptionProperties subscriptionProperties = await _adminClient.CreateSubscriptionAsync(topicName, name, cancellationToken);
        return subscriptionProperties.SubscriptionName;
    }

    public async Task DeleteSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteSubscriptionAsync(topicName, subscriptionName, cancellationToken);
    }
}
