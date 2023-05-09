// using Microsoft.OpenApi.Models;
using System.Reflection;
using EventBusExplorer.Server.Application;
using EventBusExplorer.Server.Infrastructure.AzureServiceBus;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureServiceBus(builder.Configuration);

builder.Services.AddAutoMapper(typeof(Program), typeof(Placeholder));

builder.Services.AddMvc();

builder.Services.AddSwaggerGen(options =>
{
    string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

WebApplication app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
    });
}

// required by Swagger
app.UseRouting();

#pragma warning disable
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//TODO: remove this
#pragma warning enable

await app.RunAsync();
