using Azure;
using Azure.Messaging.ServiceBus.Administration;
using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Infrastructure.AzureServiceBus;

internal class ServiceBusTopicsService : IServiceBrokerTopicsService
{
    private readonly ServiceBusAdministrationClient _adminClient;

    public ServiceBusTopicsService(
        ServiceBusAdministrationClient adminClient)
    {
        _adminClient = adminClient;
    }

    public async Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default)
    {
        TopicProperties topicProperties = await _adminClient.CreateTopicAsync(name, cancellationToken);
        return topicProperties.Name;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteTopicAsync(name, cancellationToken);
    }

    public async Task<IList<string>> GetAsync(CancellationToken cancellationToken = default)
    {
        List<string> topics = new();
        AsyncPageable<TopicProperties> topicsProperties = _adminClient.GetTopicsAsync(cancellationToken);
        await foreach (TopicProperties topicProperties in topicsProperties)
        {
            topics.Add(topicProperties.Name);
        }

        return topics;
    }

    public async Task<string> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        TopicProperties topicProperties = await _adminClient.GetTopicAsync(name, cancellationToken);
        return topicProperties.Name;
    }
}
