using System.Text;
using EventBusExplorer.Server.Infrastructure.RabbitMQ;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//using HttpMessageHandler handler = new BasicAuthHandler("guest", "guest");
using HttpClient httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:15672");
string base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"guest:guest"));
httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials);
RabbitMQAdministrationClient adminClient = new(httpClient);
RabbitMQTopicsService topicsService = new(adminClient);

await topicsService.CreateTopicAsync("sample_topic_05");
await topicsService.CreateTopicAsync("sample_topic_06");

IList<string> topics = await topicsService.GetTopicsAsync();

Console.WriteLine("Get list");

foreach (string topic in topics)
{
    Console.WriteLine(topic);
}

Console.WriteLine("Get detail");

string topic1 = await topicsService.GetTopicAsync("sample_topic_05");
Console.WriteLine(topic1);

await topicsService.DeleteTopicAsync("sample_topic_05");

topics = await topicsService.GetTopicsAsync();

Console.WriteLine("Get list");

foreach (string topic in topics)
{
    Console.WriteLine(topic);
}

Console.WriteLine("Exit..");
