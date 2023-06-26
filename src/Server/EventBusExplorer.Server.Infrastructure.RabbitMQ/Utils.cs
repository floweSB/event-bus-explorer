namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public static class Utils
{
    public static async Task ThrowExceptionIfUnsuccessfulAsync(
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
