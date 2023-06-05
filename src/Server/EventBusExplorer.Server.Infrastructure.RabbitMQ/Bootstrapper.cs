using System.Net.Http.Headers;
using System.Text;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public static class Bootstrapper
{
    /// <summary>
    /// Register Azure Service Bus as event bus
    /// </summary>
    /// <param name="services">Your <see cref="IServiceCollection"/> instance</param>
    /// <param name="configuration">Your <see cref="IConfiguration"/> instance</param>
    public static void AddRabbitMQ(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<RabbitMQAdministrationClient>(client =>
        {
            client.BaseAddress = configuration.GetValue<Uri>("RabbitMQ:Management:BaseUrl");

            string username = configuration.GetValue<string>("RabbitMQ:Management:Username")!;
            string password = configuration.GetValue<string>("RabbitMQ:Management:Password")!;

            string base64Credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{username}:{password}"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "basic",
                base64Credentials);
        });

        services.AddScoped<IServiceBrokerTopicsService, RabbitMQTopicsService>();
    }
}
