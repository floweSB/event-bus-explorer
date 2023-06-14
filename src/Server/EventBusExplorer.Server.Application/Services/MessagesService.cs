using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Application;

internal class MessagesService : IMessagesService
{
    private readonly IServiceBrokerQueuesService _queueService;

    public MessagesService(IServiceBrokerQueuesService queueService)
    {
        _queueService = queueService ??
            throw new ArgumentNullException(nameof(queueService));
    }

    public async Task<GetMessagesResponse> GetMessagesAsync(
        string queueName,
        QuerySettings querySettings,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default)
    {
        MessageListModel messages = querySettings switch
        {
            { ReceiveMode: ReceiveMode.Peek, SubQueue: SubQueue.Active } =>
                await _queueService.PeekMessagesAsync(queueName, fromSequenceNumber, cancellationToken),

            { ReceiveMode: ReceiveMode.Peek, SubQueue: SubQueue.DeadLetter } =>
                await _queueService.PeekDeadLetterMessagesAsync(queueName, fromSequenceNumber, cancellationToken),

            { ReceiveMode: ReceiveMode.Receive, SubQueue: SubQueue.Active } =>
                await _queueService.ReceiveMessagesAsync(queueName, cancellationToken),

            { ReceiveMode: ReceiveMode.Receive, SubQueue: SubQueue.DeadLetter } =>
                await _queueService.ReceiveDeadLetterMessagesAsync(queueName, cancellationToken),

            _ => throw new NotImplementedException($"Unknown settings: {querySettings.ReceiveMode}, {querySettings.SubQueue}"),
        };

        var dtos = messages.Messages
            .Select(m => new GetMessageResponse(m.SequenceNumber, m.Subject, m.Body))
            .ToList();

        return new GetMessagesResponse(dtos);
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
}
