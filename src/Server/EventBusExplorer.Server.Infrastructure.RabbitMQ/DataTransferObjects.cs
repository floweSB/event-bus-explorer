using System.Text.Json.Serialization;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

internal record ExchangeTopic(
    string Name,
    bool Durable,
    [property: JsonPropertyName("auto_delete")] bool AutoDelete,
    string Type = "topic");