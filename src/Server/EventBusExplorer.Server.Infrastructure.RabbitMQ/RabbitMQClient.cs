using RabbitMQ.Client;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public class RabbitMQClient
{
    private readonly IModel _channel;

    public RabbitMQClient(string hostname)
    {
        ConnectionFactory factory = new ConnectionFactory { HostName = hostname };
        IConnection connection = factory.CreateConnection();
        _channel = connection.CreateModel();
    }
    public string CreateQueueAndBindToExchange(string? queueName, string exchangeName)
    {
        QueueDeclareOk queueDeclare = _channel.QueueDeclare(
            queueName,
            durable: true,
            autoDelete: false,
            exclusive: false);

        _channel.QueueBind(queueName, exchangeName, routingKey: string.Empty);

        return queueDeclare.QueueName;
    }

    public void DeleteQueue(string queueName, string exchangeName)
    {
        _channel.QueueUnbind(
            queueName,
            exchangeName,
            routingKey: string.Empty);

        _channel.QueueDelete(
            queueName,
            ifUnused: false,
            ifEmpty: false);
    }
}
