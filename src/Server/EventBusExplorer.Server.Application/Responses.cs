namespace EventBusExplorer.Server.Application;

public record GetMessagesResponse(IList<GetMessageResponse> Messages);

public record GetMessageResponse(long SequenceNumber, string Subject, string Body);
