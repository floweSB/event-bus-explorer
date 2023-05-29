using System.Text.Json.Serialization;

namespace EventBusExplorer.Server.Infrastructure.RabbitMq;

internal record CreateTopicResponse(
    string Name,
    string Type,
    bool Durable,
    [property: JsonPropertyName("auto_delete")] bool AutoDelete);