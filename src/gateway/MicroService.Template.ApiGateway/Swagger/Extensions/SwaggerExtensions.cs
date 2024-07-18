using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
namespace MicroService.Template.ApiGateway.Swagger.Extensions;

public static class SwaggerExtensions
{
    public static void ConfigureSwaggerEndpoints(
        this SwaggerUIOptions options,
        ReverseProxyDocumentFilterConfig config
        )
    {
        if (config.Swagger.IsCommonDocument)
        {
            var name = config.Swagger.CommonDocumentName;
            options.SwaggerEndpoint($"/{name}/swagger/v1/swagger.json", name);
        }
        else
        {
            foreach (var cluster in config.Clusters)
            {
                var name = cluster.Key;
                options.SwaggerEndpoint($"/{name}/swagger/v1/swagger.json", name);
            }
        }
    }

    public static void ConfigureSwaggerDocs(
        this SwaggerGenOptions options,
        ReverseProxyDocumentFilterConfig config
    )
    {
        if (config.Swagger.IsCommonDocument)
        {
            var name = config.Swagger.CommonDocumentName;
            options.SwaggerDoc(name, new OpenApiInfo { Title = name, Version = name });
        }
        else
        {
            foreach (var cluster in config.Clusters)
            {
                var name = cluster.Key;
                options.SwaggerDoc(name, new OpenApiInfo { Title = name, Version = name });
            }
        }
    }
}