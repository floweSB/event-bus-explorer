using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestConsole;

public class RabbitMQTopicExchangeManager
{
    private const string RABBITMQMANAGEMENTBASEURL = "http://localhost:15672";
    private const string RABBITMQMANAGEMENTUSERNAME = "guest";
    private const string RABBITMQMANAGEMENTPASSWORD = "guest";

    private const string DEFAULTVIRTUALHOST = "/";

    public async Task CreateTopicExchangeAsync(string exchangeName, string virtualHost = DEFAULTVIRTUALHOST)
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {GetAuthorizationHeaderValue()}");

        var createUrl = $"{RABBITMQMANAGEMENTBASEURL}/api/exchanges/{Uri.EscapeDataString(virtualHost)}/{Uri.EscapeDataString(exchangeName)}";

        var requestBody = new
        {
            type = "topic",
            auto_delete = false,
            durable = true,
            name = exchangeName
        };

        string jsonContent = JsonConvert.SerializeObject(requestBody);
        using StringContent content = new(jsonContent, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PutAsync(createUrl, content);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Topic Exchange '{exchangeName}' created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create Topic Exchange '{exchangeName}'. Status code: {response.StatusCode}");
        }
    }

    public async Task DeleteTopicExchangeAsync(string exchangeName, string virtualHost = DEFAULTVIRTUALHOST)
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {GetAuthorizationHeaderValue()}");

        var deleteUrl = $"{RABBITMQMANAGEMENTBASEURL}/api/exchanges/{Uri.EscapeDataString(virtualHost)}/{Uri.EscapeDataString(exchangeName)}";

        var response = await httpClient.DeleteAsync(deleteUrl);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Topic Exchange '{exchangeName}' deleted successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to delete Topic Exchange '{exchangeName}'. Status code: {response.StatusCode}");
        }
    }

    public async Task<IList<string>> GetTopicExchangesAsync(string virtualHost = DEFAULTVIRTUALHOST)
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {GetAuthorizationHeaderValue()}");

        string getUrl = $"{RABBITMQMANAGEMENTBASEURL}/api/exchanges/{Uri.EscapeDataString(virtualHost)}";

        HttpResponseMessage response = await httpClient.GetAsync(getUrl);

        string rawResponseBody = await response.Content.ReadAsStringAsync();

        List<ExchangeTopic> topics = await Utils.ConvertResponseContentAsync<List<ExchangeTopic>>(response);

        return topics.Select(x => x.Name).ToList();
    }

    public async Task<string> GetTopicExchangeAsync(string exchangeName, string virtualHost = DEFAULTVIRTUALHOST)
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {GetAuthorizationHeaderValue()}");

        string getUrl = $"{RABBITMQMANAGEMENTBASEURL}/api/exchanges/{Uri.EscapeDataString(virtualHost)}/{Uri.EscapeDataString(exchangeName)}";

        HttpResponseMessage response = await httpClient.GetAsync(getUrl);

        string rawResponseBody = await response.Content.ReadAsStringAsync();

        ExchangeTopic topic = await Utils.ConvertResponseContentAsync<ExchangeTopic>(response);

        return topic.Name;
    }

    private static string GetAuthorizationHeaderValue()
    {
        string credentials = $"{RABBITMQMANAGEMENTUSERNAME}:{RABBITMQMANAGEMENTPASSWORD}";
        string base64Credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));

        return base64Credentials;
    }
}

public record ExchangeTopic(
    JObject Arguments,
    bool? AutoDelete,
    bool? Durable,
    bool? Internal,
    string Name,
    string Type,
    string UserWhoPerformedAction,
    string Vhost);