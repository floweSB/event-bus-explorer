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

    internal async Task CreateExchangeAsync(
        string? name,
        ExchangeType type,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Creating Exchange with empty string is not currently supported.");

        string path = GetExchangePath(virtualHost) +
            $"{Uri.EscapeDataString(name)}";

        Exchange exchange = new(
            Name: name ?? string.Empty,
            Durable: true,
            AutoDelete: false,
            Type: type);

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(
            path,
            exchange,
            cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "PUT", path);
    }

    internal async Task<IList<Exchange>> GetExchangesAsync(
        ExchangeType type,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = GetExchangePath(virtualHost);

        HttpResponseMessage response = await _httpClient.GetAsync(
            path,
            cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "GET", path);

        List<Exchange> exchanges = await response.Content.ReadFromJsonAsync<List<Exchange>>(
            cancellationToken: cancellationToken) ?? new List<Exchange>();

        return exchanges.Where(x => x.Type == type).ToList();
    }

    internal async Task<Exchange?> GetExchangeAsync(
        string name,
        ExchangeType type,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = GetExchangePath(virtualHost) +
            $"{Uri.EscapeDataString(name)}";

        HttpResponseMessage response = await _httpClient.GetAsync(
             path,
        cancellationToken: cancellationToken);

        await Utils.ThrowExceptionIfUnsuccessfulAsync(response, "GET", path);

        Exchange? exchange = await response
            .Content
            .ReadFromJsonAsync<Exchange>(
            cancellationToken: cancellationToken);

        return exchange is not null && exchange.Type == type ? exchange : null;
    }

    internal async Task DeleteExchangeAsync(
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
