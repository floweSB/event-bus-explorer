using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

internal record Exchange(
    string Name,
    bool Durable,
    [property: JsonPropertyName("auto_delete")] bool AutoDelete,
    ExchangeType Type);

[JsonConverter(typeof(JsonStringEnumMemberConverter))]
internal enum ExchangeType
{
    [EnumMember(Value = "topic")]
    Topic,
    [EnumMember(Value = "direct")]
    Direct,
    [EnumMember(Value = "fanout")]
    Fanout,
    [EnumMember(Value = "headers")]
    Headers
}
