namespace EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

public record MessageModel(
    long SequenceNumber,
    string Subject,
    string Body);

public record MessageListModel
{
    public MessageListModel(IEnumerable<MessageModel> messages)
    {
        if (messages is null)
            throw new ArgumentNullException(nameof(messages));

        Messages = new List<MessageModel>(messages).AsReadOnly();
    }

    public IReadOnlyCollection<MessageModel> Messages { get; }
}
