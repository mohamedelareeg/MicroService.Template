using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Template.Catalog.Api.Features.Products.Queries
{
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{productId}",
                async ([FromBody] GetProductByIdQuery request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request);

                    return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
                })
                .Produces<Product>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithName("GetProductById")
                .WithSummary("Get Product by ID")
                .WithDescription("Retrieve product details based on its unique ID.");
        }
    }
    public record GetProductByIdQuery(Guid ProductId) : IQuery<Product>;
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<Product>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<Product>(new Error("Product.NotFound", $"Product with ID {request.ProductId} not found."));
            }

            return Result.Success(product);
        }
    }

}
