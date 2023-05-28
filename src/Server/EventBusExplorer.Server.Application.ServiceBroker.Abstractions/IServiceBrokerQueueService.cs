namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

/// <summary>
/// Access registered event bus to manage queues
/// </summary>
public interface IServiceBrokerQueuesService
{
    /// <summary>
    /// Create a new queue
    /// </summary>
    /// <param name="name">(Optional) Queue name. If null or empty, a random one is generated</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the created queue</returns>
    Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get list of queues
    /// </summary>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>List of topics</returns>
    Task<IList<string>> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details of the given queue
    /// </summary>
    /// <param name="name">Queue name</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the queue</returns>
    Task<string> GetAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the specified queue
    /// </summary>
    /// <param name="name">Name of the queue to delete</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A task</returns>
    Task DeleteAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Peek messages from given queue
    /// </summary>
    /// <param name="queueName">Name of the queue to peek</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A list of messages</returns>
    Task<MessageListModel> PeekMessagesAsync(
        string queueName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default);
}
