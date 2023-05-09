namespace EventBusExplorer.Server.API;

public record CreateQueueResponse(string Name);

public record GetQueueResponse(string Name);

public record GetQueuesResponseItem(string Name);

public record GetQueuesResponse(IList<string> Names);
