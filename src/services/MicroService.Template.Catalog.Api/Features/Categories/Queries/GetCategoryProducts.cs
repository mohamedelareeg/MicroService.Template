using BuildingBlocks.Extentions;
using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Products;

namespace MicroService.Template.Catalog.Api.Features.Categories.Queries
{
    public class GetCategoryProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/{categoryId}/products",
                async (HttpRequest req, ISender sender, CancellationToken cancellationToken) =>
                {
                    if (!Guid.TryParse(req.RouteValues["categoryId"]?.ToString(), out var categoryId))
                    {
                        return Results.Problem("Category.GetCategoryProducts", "Invalid category ID format.");
                    }

                    var query = new GetCategoryProductsQuery(categoryId);

                    var result = await sender.Send(query);

                    return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Error);
                })
                .WithName("GetCategoryProducts")
                .Produces<CustomList<Product>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Products by Category ID")
                .WithDescription("Get all products belonging to a category by its ID.");
        }
    }
    public record GetCategoryProductsQuery(Guid CategoryId) : IListQuery<Product>;
    public class GetCategoryProductsQueryHandler : IListQueryHandler<GetCategoryProductsQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetCategoryProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<CustomList<Product>>> Handle(GetCategoryProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetByCategoryIdAsync(request.CategoryId);
            return Result.Success(products.ToCustomList());
        }
    }

}
