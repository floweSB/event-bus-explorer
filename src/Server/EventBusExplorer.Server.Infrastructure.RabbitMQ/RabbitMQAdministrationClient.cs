using System.Net.Http.Json;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public class RabbitMQAdministrationClient
{
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
        // TODO: centralize paths
        string path = $"/api/exchanges/{Uri.EscapeDataString(virtualHost)}/" +
            $"{Uri.EscapeDataString(name)}";

        ExchangeTopic requestTopic = new(
            Name: name ?? string.Empty,
            Durable: true,
            AutoDelete: false);

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(path, requestTopic, cancellationToken: cancellationToken);

        await ThrowExceptionIfUnsuccessfulAsync(response, "PUT", path);
    }

    internal async Task<IList<ExchangeTopic>> GetTopicsAsync(
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = $"/api/exchanges/{Uri.EscapeDataString(virtualHost)}";

        HttpResponseMessage response = await _httpClient.GetAsync(path, cancellationToken: cancellationToken);

        await ThrowExceptionIfUnsuccessfulAsync(response, "GET", path);

        List<ExchangeTopic>? payload = await response.Content.ReadFromJsonAsync<List<ExchangeTopic>>(
            cancellationToken: cancellationToken);

        return payload!;
    }

    internal async Task<ExchangeTopic> GetTopicAsync(
        string name,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = $"/api/exchanges/{Uri.EscapeDataString(virtualHost)}/" +
            $"{Uri.EscapeDataString(name)}";

        HttpResponseMessage response = await _httpClient.GetAsync(
            path,
            cancellationToken: cancellationToken);

        await ThrowExceptionIfUnsuccessfulAsync(response, "GET", path);

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
        string path = $"/api/exchanges/{Uri.EscapeDataString(virtualHost)}/" +
            $"{Uri.EscapeDataString(name)}";

        HttpResponseMessage response = await _httpClient.DeleteAsync(
            path,
            cancellationToken: cancellationToken);

        await ThrowExceptionIfUnsuccessfulAsync(response, "DELETE", path);
    }

    //TODO: find a way to centralize as a common utility
    private static async Task ThrowExceptionIfUnsuccessfulAsync(
        HttpResponseMessage response,
        string httpVerb,
        string path)
    {
        if (!response.IsSuccessStatusCode)
        {
            string payload = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"{httpVerb} {path} -> {response.StatusCode}. " +
                $"Payload: {payload}");
        }
    }
}