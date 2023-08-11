using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Application;

internal class MessagesService : IMessagesService
{
    private readonly IServiceBrokerQueuesService _queuesService;
    private readonly IServiceBrokerTopicsService _topicsService;

    public MessagesService(
        IServiceBrokerQueuesService queueService,
        IServiceBrokerTopicsService topicsService)
    {
        _queuesService = queueService ??
            throw new ArgumentNullException(nameof(queueService));

        _topicsService = topicsService ??
            throw new ArgumentNullException(nameof(topicsService));
    }

    public async Task<GetMessagesResponse> GetMessagesAsync(
        string queueName,
        QuerySettings querySettings,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        MessageList messages = querySettings switch
        {
            { ReceiveMode: ReceiveMode.Peek, SubQueue: SubQueue.Active } =>
                await _queuesService.PeekMessagesAsync(queueName, fromSequenceNumber, cancellationToken),

            { ReceiveMode: ReceiveMode.Peek, SubQueue: SubQueue.DeadLetter } =>
                await _queuesService.PeekDeadLetterMessagesAsync(queueName, fromSequenceNumber, cancellationToken),

            { ReceiveMode: ReceiveMode.Receive, SubQueue: SubQueue.Active } =>
                await _queuesService.ReceiveMessagesAsync(queueName, cancellationToken),

            { ReceiveMode: ReceiveMode.Receive, SubQueue: SubQueue.DeadLetter } =>
                await _queuesService.ReceiveDeadLetterMessagesAsync(queueName, cancellationToken),

            _ => throw new NotImplementedException($"Unknown settings: {querySettings.ReceiveMode}, {querySettings.SubQueue}"),
        };

        var dtos = messages.Messages
            .Select(m => new GetMessageResponse(m.SequenceNumber, m.Subject, m.Body))
            .ToList();

        return new GetMessagesResponse(dtos);
    }

    public async Task<GetMessagesResponse> GetMessagesAsync(
        string topicName,
        string subscriptionName,
        QuerySettings querySettings,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        MessageList messages = querySettings switch
        {
            { ReceiveMode: ReceiveMode.Peek, SubQueue: SubQueue.Active } =>
                await _topicsService.PeekMessagesAsync(topicName, subscriptionName, fromSequenceNumber, cancellationToken),

            { ReceiveMode: ReceiveMode.Peek, SubQueue: SubQueue.DeadLetter } =>
                await _topicsService.PeekDeadLetterMessagesAsync(topicName, subscriptionName, fromSequenceNumber, cancellationToken),

            { ReceiveMode: ReceiveMode.Receive, SubQueue: SubQueue.Active } =>
                await _topicsService.ReceiveMessagesAsync(topicName, subscriptionName, cancellationToken),

            { ReceiveMode: ReceiveMode.Receive, SubQueue: SubQueue.DeadLetter } =>
                await _topicsService.ReceiveDeadLetterMessagesAsync(topicName, subscriptionName, cancellationToken),

            _ => throw new NotImplementedException($"Unknown settings: {querySettings.ReceiveMode}, {querySettings.SubQueue}"),
        };

        var dtos = messages.Messages
            .Select(m => new GetMessageResponse(m.SequenceNumber, m.Subject, m.Body))
            .ToList();

        return new GetMessagesResponse(dtos);
    }

    public async Task PurgeMessagesAsync(
        string topicName,
        string subscriptionName,
        SubQueue subQueue,
        CancellationToken cancellationToken = default)
    {
        var task = subQueue switch
        {
            SubQueue.Active =>
                _topicsService.PurgeMessagesAsync(topicName, subscriptionName, cancellationToken),

            SubQueue.DeadLetter =>
                _topicsService.PurgeDeadLetterMessagesAsync(topicName, subscriptionName, cancellationToken),

            _ => throw new NotImplementedException($"Unknown subqueue: {subQueue}"),
        };

        await task;
    }

    public async Task PurgeMessagesAsync(
    string queueName,
    SubQueue subQueue,
    CancellationToken cancellationToken = default)
    {
        var task = subQueue switch
        {
            SubQueue.Active =>
                _queuesService.PurgeMessagesAsync(queueName, cancellationToken),

            SubQueue.DeadLetter =>
                _queuesService.PurgeDeadLetterMessagesAsync(queueName, cancellationToken),

            _ => throw new NotImplementedException($"Unknown subqueue: {subQueue}"),
        };

        await task;
    }
}

public interface IMessagesService
{
    /// <summary>
    /// Retrieve messages from a queue. Messages can be peeked or received
    /// </summary>
    /// <param name="queueName">Queue name</param>
    /// <param name="querySettings">Set of retrieval settings</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A list of messages</returns>
    Task<GetMessagesResponse> GetMessagesAsync(
        string queueName,
        QuerySettings querySettings,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve messages from a topic subscription. Messages can be peeked or received
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Topic subscription name</param>
    /// <param name="querySettings">Set of retrieval settings</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A list of messages</returns>
    Task<GetMessagesResponse> GetMessagesAsync(
        string topicName,
        string subscriptionName,
        QuerySettings querySettings,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Purge a queue
    /// </summary>
    /// <param name="queueName">Queue name</param>
    /// <param name="subQueue">Sub queue to query</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    Task PurgeMessagesAsync(
        string queueName,
        SubQueue subQueue,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Purge a topic subscription
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Topic subscription name</param>
    /// <param name="subQueue">Sub queue to query</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    Task PurgeMessagesAsync(
        string topicName,
        string subscriptionName,
        SubQueue subQueue,
        CancellationToken cancellationToken = default);
}
