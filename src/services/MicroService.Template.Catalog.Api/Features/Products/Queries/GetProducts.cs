using BuildingBlocks.Extentions;
using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Template.Catalog.Api.Features.Products.Queries
{
    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products",
                async ([FromBody] GetProductsQuery request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request);

                    return Results.Ok(result.Value);
                })
                .Produces<CustomList<Product>>(StatusCodes.Status200OK)
                .WithName("GetProducts")
                .WithSummary("Get All Products")    
                .WithDescription("Retrieve all products available.");
        }
    }

    public record GetProductsQuery() : IListQuery<Product>;
    public class GetProductsQueryHandler : IListQueryHandler<GetProductsQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<CustomList<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync();
            return Result.Success(products.ToCustomList());
        }
    }

}
