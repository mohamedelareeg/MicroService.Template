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
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{categoryId}",
                async ([FromBody] GetProductByCategoryQuery request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request);

                    return result.IsSuccess ? Results.Ok(result.Value) : Results.NotFound(result.Error);
                })
                .Produces<CustomList<Product>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithName("GetProductByCategory")
                .WithSummary("Get Products by Category")
                .WithDescription("Retrieve products belonging to a specific category by its ID.");
        }
    }
    public record GetProductByCategoryQuery(Guid CategoryId) : IListQuery<Product>;
    public class GetProductByCategoryQueryHandler : IListQueryHandler<GetProductByCategoryQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByCategoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<CustomList<Product>>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByCategoryIdAsync(request.CategoryId);
            return Result.Success(products.ToCustomList());
        }
    }

}
