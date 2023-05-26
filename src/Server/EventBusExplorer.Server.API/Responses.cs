namespace EventBusExplorer.Server.API;

#region Queues

/// <summary>
/// Description of the created queue
/// </summary>
/// <param name="Name">Name of the created queue</param>
public record CreateQueueResponse(string Name);

/// <summary>
/// Details of the queue
/// </summary>
/// <param name="Name">Name of the queue</param>
public record GetQueueResponse(string Name);

/// <summary>
/// List of queues
/// </summary>
/// <param name="Names">List of queues names</param>
public record GetQueuesResponse(IList<string> Names);

#endregion

#region Topics

/// <summary>
/// Description of the created topic
/// </summary>
/// <param name="Name">Name of the created topic</param>
public record CreateTopicResponse(string Name);

/// <summary>
/// List of topics
/// </summary>
/// <param name="Names">List of topics names</param>
public record GetTopicsResponse(IList<string> Names);

/// <summary>
/// Details of the topic
/// </summary>
/// <param name="Name">Name of the topic</param>
public record GetTopicResponse(string Name);

/// <summary>
/// Get list of subscriptions
/// </summary>
/// <param name="Names">List of subscription names</param>
public record GetTopicSubscriptionsResponse(IList<string> Names);

/// <summary>
/// Details of the subscription
/// </summary>
/// <param name="Name">Name of the topic</param>
public record GetTopicSubscriptionResponse(string Name);

/// <summary>
/// Details of the created subscription
/// </summary>
/// <param name="Name">Name of the created subscription</param>
public record CreateSubscriptionResponse(string Name);

#endregion
