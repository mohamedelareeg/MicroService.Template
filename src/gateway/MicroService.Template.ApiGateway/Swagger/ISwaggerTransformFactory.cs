using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace MicroService.Template.ApiGateway.Swagger
{
    public interface ISwaggerTransformFactory
    {
        bool Build(OpenApiOperation operation, IReadOnlyDictionary<string, string> transformValues);
    }
}
