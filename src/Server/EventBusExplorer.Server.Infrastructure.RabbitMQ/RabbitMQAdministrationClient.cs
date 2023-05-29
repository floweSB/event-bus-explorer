using System.Net.Http.Json;

namespace EventBusExplorer.Server.Infrastructure.RabbitMq;

public class RabbitMQAdministrationClient
{
    private readonly HttpClient _httpClient;

    public RabbitMQAdministrationClient(HttpClient? httpClient)
    {
        _httpClient = httpClient!;
    }

    public async Task<string> CreateTopicAsync(
        string name,
        string virtualHost = "/",
        CancellationToken cancellationToken = default)
    {
        string path = $"/api/exchanges/{Uri.EscapeDataString(virtualHost)}/" +
            $"{Uri.EscapeDataString(name)}";

        CreateTopicRequest request = new(name);

        HttpResponseMessage response = await _httpClient.PutAsJsonAsync(path, request, cancellationToken: cancellationToken);

        //TODO: consider centralize code
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"PUT {path} -> {response.StatusCode}. Payload: {response.Content.ReadAsStringAsync()}");
        }

        CreateTopicResponse? payload = await response.Content.ReadFromJsonAsync<CreateTopicResponse>(
            cancellationToken: cancellationToken);

        return payload!.Name;
    }

    public Task<string> GetTopicsAsync(string virtualHost = "/", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetTopicAsync(string name, string virtualHost = "/", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTopicAsync(string name, string virtualHost = "/", CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}