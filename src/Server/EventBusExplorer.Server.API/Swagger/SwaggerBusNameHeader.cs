using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventBusExplorer.Server.API.Swagger;

public class SwaggerBusNameHeader : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "x-bus",
            In = ParameterLocation.Header,
            Description = "Description",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string",
            }
        });
    }
}
