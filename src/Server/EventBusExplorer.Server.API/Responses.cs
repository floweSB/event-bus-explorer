namespace EventBusExplorer.Server.API;

#region Queues

public record CreateQueueResponse(string Name);

public record GetQueueResponse(string Name);

public record GetQueuesResponse(IList<string> Names);

#endregion

#region Topics

public record CreateTopicResponse(string Name);

public record GetTopicsResponse(IList<string> Names);

public record GetTopicResponse(string Name);

public record GetTopicSubscriptionsResponse(IList<string> Names);

public record GetTopicSubscriptionResponse(string Name);

public record CreateSubscriptionResponse(string Name);

#endregion
