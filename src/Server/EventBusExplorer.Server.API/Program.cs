using EventBusExplorer.Server.Infrastructure.AzureServiceBus;
using Microsoft.OpenApi.Models;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureServiceBus(builder.Configuration);

builder.Services.AddMvc();

builder.Services.AddSwaggerGen(options =>
{
    //TODO: set below info
    var contact = new OpenApiContact
    {
        // Name = _configuration["SwaggerApiInfo:Name"],
        // Email = _configuration["SwaggerApiInfo:Email"],
        // Url = new Uri(_configuration["SwaggerApiInfo:Uri"])
        Name = ""
    };

    //TODO: set below infos
    options.SwaggerDoc(
        "",
        new OpenApiInfo
        {
            Title = "",
            Contact = contact
        });

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
