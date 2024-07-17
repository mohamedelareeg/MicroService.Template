using BuildingBlocks.Extentions;
using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Categories;

namespace MicroService.Template.Catalog.Api.Features.Categories.Queries
{
    public class GetCategoriesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories",
                async (HttpRequest req, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetCategoriesQuery();

                    var result = await sender.Send(query);

                    return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Error);
                })
                .WithName("GetCategories")
                .Produces<CustomList<Category>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Get Categories")
                .WithDescription("Get all categories.");
        }
    }
    public record GetCategoriesQuery() : IListQuery<Category>;
    public class GetCategoriesQueryHandler : IListQueryHandler<GetCategoriesQuery, Category>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CustomList<Category>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            return Result.Success(categories.ToCustomList());
        }
    }

}
