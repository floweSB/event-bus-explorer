using System.Reflection;
using System.Text.Json.Serialization;
using EventBusExplorer.Server.Application;
using EventBusExplorer.Server.Infrastructure.AzureServiceBus;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureServiceBus(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddAutoMapper(typeof(Program), typeof(Placeholder));

builder.Services.AddMvc();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddRouting(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services.AddSwaggerGen(options =>
{
    ConfigurationManager config = builder.Configuration;

    var contact = new OpenApiContact
    {
        Name = config["SwaggerApiInfo:Name"],
        Url = new Uri(config["SwaggerApiInfo:Uri"]!),
    };

    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = $"{config["SwaggerApiInfo:Title"]}",
            Version = "v1",
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
