namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

public record Message(
    long SequenceNumber,
    string Subject,
    string Body);

public record MessageList
{
    public MessageList(IEnumerable<Message> messages)
    {
        if (messages is null)
            throw new ArgumentNullException(nameof(messages));

        Messages = new List<Message>(messages).AsReadOnly();
    }

    public IReadOnlyCollection<Message> Messages { get; }
}
