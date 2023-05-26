namespace EventBusExplorer.Server.API;

/// <summary>
/// Details of the queue to create
/// </summary>
/// <param name="Name">Queue name</param>
public record CreateQueueRequest(string? Name);

/// <summary>
/// Details of the topic to create
/// </summary>
/// <param name="Name">Topic name</param>
public record CreateTopicRequest(string? Name);

/// <summary>
/// Details of the subscription to create
/// </summary>
/// <param name="Name">Subscription name</param>
public record CreateSubscriptionRequest(string? Name);
