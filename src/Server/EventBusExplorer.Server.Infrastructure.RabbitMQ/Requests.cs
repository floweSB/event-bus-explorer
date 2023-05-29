using System.Text.Json.Serialization;

namespace EventBusExplorer.Server.Infrastructure.RabbitMq;

internal record CreateTopicRequest(
    string Name,
    string Type = "topic",
    bool Durable = true,
    [property: JsonPropertyName("auto_delete")] bool AutoDelete = false);