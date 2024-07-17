using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Categories;

namespace MicroService.Template.Catalog.Api.Features.Categories.Queries
{
    public class GetCategoryByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/{id}",
                async (HttpRequest req, ISender sender, CancellationToken cancellationToken) =>
                {
                    if (!Guid.TryParse(req.RouteValues["id"]?.ToString(), out var categoryId))
                    {
                        return Results.Problem("Category.GetCategory", "Invalid category ID format.");
                    }

                    var query = new GetCategoryByIdQuery(categoryId);

                    var result = await sender.Send(query);

                    return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Error);
                })
                .WithName("GetCategoryById")
                .Produces<Category>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Category by ID")
                .WithDescription("Get a category by its ID.");
        }
    }
    public record GetCategoryByIdQuery(Guid CategoryId) : IQuery<Category>;
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<Category>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return Result.Failure<Category>(new Error("Category.NotFound", $"Category with ID {request.CategoryId} not found."));
            }

            return Result.Success(category);
        }
    }

}
