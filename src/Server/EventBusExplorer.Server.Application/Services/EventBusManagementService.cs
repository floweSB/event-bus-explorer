using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Application;

internal class EventBusManagementService : IEventBusManagementService
{
    private readonly IServiceBrokerQueuesService _queuesService;
    private readonly IServiceBrokerTopicsService _topicsService;

    public EventBusManagementService(
        IServiceBrokerQueuesService queueService,
        IServiceBrokerTopicsService topicsService)
    {
        _queuesService = queueService ??
            throw new ArgumentNullException(nameof(queueService));

        _topicsService = topicsService ??
            throw new ArgumentNullException(nameof(topicsService));
    }

    public Task<string> CreateQueueAsync(string? name, CancellationToken cancellationToken = default) =>
        _queuesService.CreateAsync(name, cancellationToken);

    public Task<string> CreateTopicAsync(string? name, CancellationToken cancellationToken = default) =>
        _topicsService.CreateTopicAsync(name, cancellationToken);

    public Task<string> CreateTopicSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default) =>
        _topicsService.CreateSubscriptionAsync(topicName, name, cancellationToken);

    public Task DeleteQueueAsync(string name, CancellationToken cancellationToken = default) =>
        _queuesService.DeleteAsync(name, cancellationToken);

    public Task DeleteTopicAsync(string name, CancellationToken cancellationToken = default) =>
        _topicsService.DeleteTopicAsync(name, cancellationToken);

    public Task DeleteTopicSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default) =>
        _topicsService.DeleteSubscriptionAsync(topicName, subscriptionName, cancellationToken);

    public Task<string> GetQueueAsync(string name, CancellationToken cancellationToken = default) =>
        _queuesService.GetAsync(name, cancellationToken);

    public Task<IList<string>> GetQueuesAsync(CancellationToken cancellationToken = default) =>
        _queuesService.GetAsync(cancellationToken);

    public Task<string> GetTopicAsync(string name, CancellationToken cancellationToken = default) =>
        _topicsService.GetTopicAsync(name, cancellationToken);

    public Task<IList<string>> GetTopicsAsync(CancellationToken cancellationToken = default) =>
        _topicsService.GetTopicsAsync(cancellationToken);

    public Task<string> GetTopicSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default) =>
        _topicsService.GetSubscriptionAsync(topicName, subscriptionName, cancellationToken);

    public Task<IList<string>> GetTopicSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default) =>
        _topicsService.GetSubscriptionsAsync(topicName, cancellationToken);
}

public interface IEventBusManagementService
{
    /// <summary>
    /// Create a new queue
    /// </summary>
    /// <param name="name">(Optional) Queue name. If null or empty, a random one is generated</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the created queue</returns>
    Task<string> CreateQueueAsync(string? name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get list of queues
    /// </summary>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>List of topics</returns>
    Task<IList<string>> GetQueuesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details of the given queue
    /// </summary>
    /// <param name="name">Queue name</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the queue</returns>
    Task<string> GetQueueAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the specified queue
    /// </summary>
    /// <param name="name">Name of the queue to delete</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A task</returns>
    Task DeleteQueueAsync(string name, CancellationToken cancellationToken = default);

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
    Task<IList<string>> GetTopicSubscriptionsAsync(string topicName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get details of the specified subscription
    /// </summary>
    /// <param name="topicName">Topic to fetch subscription from</param>
    /// <param name="subscriptionName">Subscription to get details</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the subscription</returns>
    Task<string> GetTopicSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new subscription in specified topic
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="name">(Optional) Name of the new subscription. If null or empty, a random one is generated</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>Name of the created subscription</returns>
    Task<string> CreateTopicSubscriptionAsync(string topicName, string? name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete the specified subscription in the given topic
    /// </summary>
    /// <param name="topicName">Topic name</param>
    /// <param name="subscriptionName">Name of the subscription to delete</param>
    /// <param name="cancellationToken">(Optional) Cancellation token to cancel the operation</param>
    /// <returns>A task</returns>
    Task DeleteTopicSubscriptionAsync(string topicName, string subscriptionName, CancellationToken cancellationToken = default);
}
