using EventBusExplorer.Server.Application.ServiceBroker.Abstractions;

namespace EventBusExplorer.Server.Infrastructure.RabbitMQ;

public class RabbitMQQueuesService : IServiceBrokerQueuesService
{
    private readonly RabbitMQAdministrationClient _adminClient;

    public RabbitMQQueuesService(
        RabbitMQAdministrationClient adminClient
    )
    {
        _adminClient = adminClient;
    }

    public async Task<string> CreateAsync(string? name, CancellationToken cancellationToken = default)
    {
        await _adminClient.CreateExchangeAsync(
            name,
            type: ExchangeType.Direct,
            cancellationToken: cancellationToken);

        //
        // Since exchange creation through management API does not return anything,
        // It is assumed that if the request is successful then the exchange has been created.
        //
        return name!;
    }

    public async Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        await _adminClient.DeleteExchangeAsync(name, cancellationToken: cancellationToken);
    }

    public async Task<IList<string>> GetAsync(CancellationToken cancellationToken = default)
    {
        IList<Exchange> directExchanges = await _adminClient.GetExchangesAsync(
            type: ExchangeType.Direct,
            cancellationToken: cancellationToken);

        return directExchanges.Select(x => x.Name).ToList();
    }

    public async Task<string> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        Exchange? directExchange = await _adminClient.GetExchangeAsync(
            name,
            type: ExchangeType.Direct,
            cancellationToken: cancellationToken);

        if (directExchange is null)
            throw new Exception($"Cannot find exchange with name: {name}"); //TODO: define or find a suitable exc type

        return directExchange.Name;
    }
}
