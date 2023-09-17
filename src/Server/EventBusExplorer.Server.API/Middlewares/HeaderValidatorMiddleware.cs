using EventBusExplorer.Server.API.Exceptions;
using EventBusExplorer.Server.Infrastructure.AzureServiceBus;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace EventBusExplorer.Server.API.Middlewares;

public class HeaderValidatorMiddleware : IMiddleware
{
    private readonly ILogger<HeaderValidatorMiddleware> _logger;
    private readonly IOptions<EventBusSettings> _settings;

    public HeaderValidatorMiddleware(
        IOptions<EventBusSettings> settings,
        ILogger<HeaderValidatorMiddleware> logger)
    {
        _logger = logger;
        _settings = settings;
    }

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        try
        {
            if (context.Request.Headers.TryGetValue("x-bus", out StringValues busName))
            {
                _logger.LogDebug("Found bus {name} in request", busName!);

                if (!_settings.Value.ServiceBusNames.Contains(busName!))
                {
                    _logger.LogError("Invalid bus name {busName}", busName!);
                    throw new InvalidBusException(busName!);
                }
            }
            else
            {
                _logger.LogDebug("No bus name found in request");

                if (_settings.Value.ServiceBusNames.Count > 1)
                {
                    throw new MissingBusException();
                }
            }
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

            _logger.LogError(e, "{Message}", e.Message);

            string exceptionName = e.GetType().Name;
            int index = exceptionName.IndexOf("Exception");

            var responsePayload = new
            {
                Code = exceptionName.Remove(index),
                Description = e.Message,
            };

            await context.Response.WriteAsJsonAsync(responsePayload);
            return;
        }

        await next(context);
    }
}

public static class HeaderValidatorMiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderValidator(this IApplicationBuilder builder) =>
        builder.UseWhen(context =>
            !context.Request.Path.StartsWithSegments("/swagger"),
            appBuilder =>
            {
                appBuilder.UseMiddleware<HeaderValidatorMiddleware>();
            });
}
