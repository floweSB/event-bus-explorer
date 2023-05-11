namespace EventBusExplorer.Server.API;

public record CreateQueueRequest(string? Name);

public record CreateTopicRequest(string? Name);

public record CreateSubscriptionRequest(string? Name);
