namespace EventBusExplorer.Server.Application.ServiceBusBroker.Abstraction;

public record CreateQueueResponse(string Name);

public record GetQueueResponse(string Name);

public record GetQueuesResponseItem(string Name);

public record GetQueuesResponse(List<GetQueuesResponseItem> Items);
