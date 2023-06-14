using Microsoft.Extensions.DependencyInjection;

namespace EventBusExplorer.Server.Application;

public static class Bootstrapper
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IMessagesService, MessagesService>();

        return services;
    }
}
