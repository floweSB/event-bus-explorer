namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

/// <summary>
/// Access registered event bus to manage topics and subscriptions
/// </summary>
public interface IServiceBrokerTopicsService
{
    /// <summary>
    /// Create a new topic
    /// </summary>
    /// <param name="name">(Optional) Topic name. If null or empty, a random one is generated</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns></returns>
    Task<string> CreateTopicAsync(string? name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get list of topics
    /// </summary>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>List of topics</returns>
    Task<IList<string>> GetTopicsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details of the specified topic
    /// </summary>
    /// <param name="name">Topic name</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the topic</returns>
    Task<string> GetTopicAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete specified topic
    /// </summary>
    /// <param name="name">Name of topic to delete</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A task</returns>
    Task DeleteTopicAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get subscriptions of the specified topic
    /// </summary>
    /// <param name="topicName">Topic to fetch subscriptions from</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>List of subscription names</returns>
    Task<IList<string>> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details of the specified subscription
    /// </summary>
    /// <param name="topicName">Topic to fetch subscription from</param>
    /// <param name="subscriptionName">Subscription to get details</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the subscription</returns>
    Task<string> GetSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new subscription in specified topic
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="name">(Optional) Name of the new subscription. If null or empty, a random one is generated</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the created subscription</returns>
    Task<string> CreateSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the specified subscription in the given topic
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Name of the subscription to delete</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A task</returns>
    Task DeleteSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Peek messages from given topic subscription
    /// </summary>
    /// <param name="topicName">Name of the topic to peek</param>
    /// <param name="susbcriptionName">Name of the topic susbcription to peek</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A list of messages</returns>
    Task<MessageList> PeekMessagesAsync(
        string topicName,
        string susbcriptionName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Peek dead letter messages from given topic subscription
    /// </summary>
    /// <param name="topicName">Name of the topic to peek</param>
    /// <param name="susbcriptionName">Name of the topic susbcription to peek</param>
    /// <param name="fromSequenceNumber">(Optional) Fetch messages from this one</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A list of messages</returns>
    Task<MessageList> PeekDeadLetterMessagesAsync(
        string topicName,
        string susbcriptionName,
        long? fromSequenceNumber = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Receive messages from given topic subscription
    /// </summary>
    /// <param name="topicName">Name of the topic to peek</param>
    /// <param name="susbcriptionName">Name of the topic susbcription to peek</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A list of messages</returns>
    Task<MessageList> ReceiveMessagesAsync(
        string topicName,
        string susbcriptionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Receive dead letter messages from given topic subscription
    /// </summary>
    /// <param name="topicName">Name of the topic to peek</param>
    /// <param name="susbcriptionName">Name of the topic susbcription to peek</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    Task<MessageList> ReceiveDeadLetterMessagesAsync(
        string topicName,
        string susbcriptionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Purge topic subscription
    /// </summary>
    /// <param name="topicName">Name of the topic to purge</param>
    /// <param name="subscriptionName">Name of the topic subscription to purge</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    Task PurgeMessagesAsync(
        string topicName,
        string subscriptionName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Purge dead letter topic subscription
    /// </summary>
    /// <param name="topicName">Name of the topic to purge</param>
    /// <param name="subscriptionName">Name of the topic subscription to purge</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    Task PurgeDeadLetterMessagesAsync(
        string topicName,
        string subscriptionName,
        CancellationToken cancellationToken = default);
}
