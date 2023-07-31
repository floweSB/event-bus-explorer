using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
using EventBusExplorer.Server.Infrastructure.AzureServiceBus.Helpers;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class ServiceBusQueuesService : IServiceBrokerQueuesService
{
    private readonly ServiceBusAdministrationClient _adminClient;
    private readonly ServiceBusClient _client;

    public ServiceBusQueuesService(
        ServiceBusClient client,
        ServiceBusAdministrationClient adminClient)
    {
        _client = client ??
            throw new ArgumentNullException(nameof(client));

        _adminClient = adminClient ??
            throw new ArgumentNullException(nameof(adminClient));
    }

    public async Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.CreateQueueAsync(name, cancellationToken);
        return queueProperties.Name;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteQueueAsync(name, cancellationToken);
    }

    public async Task<IList<string>> GetAsync(CancellationToken cancellationToken = default)
    {
        List<string> queues = new();
        AsyncPageable<QueueProperties> queuesProperties = _adminClient.GetQueuesAsync(cancellationToken);
        await foreach (QueueProperties queueProperties in queuesProperties)
        {
            queues.Add(queueProperties.Name);
        }

        return queues;
    }

    public async Task<string> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        QueueProperties queueProperties = await _adminClient.GetQueueAsync(name, cancellationToken);
        return queueProperties.Name;
    }

    public async Task<MessageList> PeekMessagesAsync(
        string queueName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetReceiver(queueName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.PeekMessagesAsync(
            MAX_COUNT,
            fromSequenceNumber,
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    public async Task<MessageList> PeekDeadLetterMessagesAsync(
        string queueName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetDeadLetterReceiver(queueName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.PeekMessagesAsync(
            MAX_COUNT,
            fromSequenceNumber,
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    public async Task<MessageList> ReceiveMessagesAsync(string queueName, CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetReceiver(queueName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(
            MAX_COUNT,
            maxWaitTime: TimeSpan.FromSeconds(1),
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    public async Task<MessageList> ReceiveDeadLetterMessagesAsync(string queueName, CancellationToken cancellationToken = default)
    {
        const int MAX_COUNT = 50;

        var receiver = GetDeadLetterReceiver(queueName);

        IReadOnlyList<ServiceBusReceivedMessage> messages = await receiver.ReceiveMessagesAsync(
            MAX_COUNT,
            maxWaitTime: TimeSpan.FromSeconds(1),
            cancellationToken: cancellationToken);

        var toReturn = messages
            .Select(m => new Message(m.SequenceNumber, m.Subject, MessagesHelper.ReadMessage(m.Body)));

        return new MessageList(toReturn);
    }

    private ServiceBusReceiver GetReceiver(string name) =>
        _client.CreateReceiver(name);

    private ServiceBusReceiver GetDeadLetterReceiver(string name) =>
        _client.CreateReceiver(name, new ServiceBusReceiverOptions
        {
            SubQueue = SubQueue.DeadLetter
        });
}
