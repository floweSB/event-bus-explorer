using System.Net.Http.Json;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public class RabbitMQAdministrationClient
{
    private const string EXCHANGE_PATH = "/api/exchanges/";

    private readonly HttpClient _httpClient;

    public RabbitMQAdministrationClient(HttpClient? httpClient)
    {
        _httpClient = httpClient!;
    }

    internal async Task CreateTopicAsync(
        string? name,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Creating Exchange with empty string is not currently supported.");

        string path = GetExchangePath(virtualHost) +
            $"{Uri.EscapeDataString(name)}";

        ExchangeTopic requestTopic = new(
            Name: name ?? string.Empty,
            Durable: true,
            AutoDelete: false);

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            path,
            requestTopic,
            cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "PUT", path);
    }

    internal async Task<IList<ExchangeTopic>> GetTopicsAsync(
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = GetExchangePath(virtualHost);

        HttpResponseMessage response = await _httpClient.GetAsync(
            path,
            cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "GET", path);

        List<ExchangeTopic>? payload = await response.Content.ReadFromJsonAsync<List<ExchangeTopic>>(
            cancellationToken: cancellationToken);

        return payload!;
    }

    internal async Task<ExchangeTopic> GetTopicAsync(
        string name,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = GetExchangePath(virtualHost) +
            $"{Uri.EscapeDataString(name)}";

        HttpResponseMessage response = await _httpClient.GetAsync(
            path,
            cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "GET", path);

        ExchangeTopic? topic = await response
            .Content
            .ReadFromJsonAsync<ExchangeTopic>(
                cancellationToken: cancellationToken);

        return topic!;
    }

    internal async Task DeleteTopicAsync(
        string name,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = GetExchangePath(virtualHost) +
            $"{Uri.EscapeDataString(name)}";

        HttpResponseMessage response = await _httpClient.DeleteAsync(
            path,
            cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "DELETE", path);
    }

    private static string GetExchangePath(string virtualHost = "/")
    {
        return EXCHANGE_PATH + $"{Uri.EscapeDataString(virtualHost)}/";
    }
}