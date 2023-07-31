using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
using EventBusExplorer.Server.Infrastructure.AzureServiceBus.Helpers;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class ServiceBusTopicsService : IServiceBrokerTopicsService
{
    private readonly ServiceBusAdministrationClient _adminClient;
    private readonly ServiceBusClient _client;

    public ServiceBusTopicsService(
        ServiceBusClient client,
        ServiceBusAdministrationClient adminClient)
    {
        _client = client ??
            throw new ArgumentNullException(nameof(client));

        _adminClient = adminClient ??
            throw new ArgumentNullException(nameof(adminClient));
    }

    public async Task<string> CreateTopicAsync(string? name, CancellationToken cancellationToken = default)
    {
        TopicProperties topicProperties = await _adminClient.CreateTopicAsync(name, cancellationToken);
        return topicProperties.Name;
    }

    public async Task DeleteTopicAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteTopicAsync(name, cancellationToken);
    }

    public async Task<IList<string>> GetTopicsAsync(CancellationToken cancellationToken = default)
    {
        List<string> topics = new();
        AsyncPageable<TopicProperties> topicsProperties = _adminClient.GetTopicsAsync(cancellationToken);
        await foreach (TopicProperties topicProperties in topicsProperties)
        {
            topics.Add(topicProperties.Name);
        }

        return topics;
    }

    public async Task<string> GetTopicAsync(string name, CancellationToken cancellationToken = default)
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

    public async Task<MessageList> PeekMessagesAsync(
        string topicName,
        string susbcriptionName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetReceiver(topicName, susbcriptionName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.PeekMessagesAsync(
            MAX_COUNT,
            fromSequenceNumber,
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    public async Task<MessageList> PeekDeadLetterMessagesAsync(
        string topicName,
        string susbcriptionName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetDeadLetterReceiver(topicName, susbcriptionName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.PeekMessagesAsync(
            MAX_COUNT,
            fromSequenceNumber,
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    public async Task<MessageList> ReceiveMessagesAsync(
        string topicName,
        string susbcriptionName,
        CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetReceiver(topicName, susbcriptionName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(
            MAX_COUNT,
            maxWaitTime: TimeSpan.FromSeconds(1),
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    public async Task<MessageList> ReceiveDeadLetterMessagesAsync(
        string topicName,
        string susbcriptionName,
        CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetDeadLetterReceiver(topicName, susbcriptionName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(
            MAX_COUNT,
            maxWaitTime: TimeSpan.FromSeconds(1),
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    private ServiceBusReceiver GetReceiver(string topicName, string subscriptionName) =>
        _client.CreateReceiver(topicName, subscriptionName);

    private ServiceBusReceiver GetDeadLetterReceiver(string topicName, string subscriptionName) =>
        _client.CreateReceiver(topicName, subscriptionName, new ServiceBusReceiverOptions
        {
            SubQueue = SubQueue.DeadLetter
        });
}
