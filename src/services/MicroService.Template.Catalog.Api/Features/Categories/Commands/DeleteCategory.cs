using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.PostgreSQL.Abstractions;

namespace MicroService.Template.Catalog.Api.Features.Categories.Commands
{
    public class DeleteCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/categories/{categoryId}",
                async (HttpRequest req, ISender sender, CancellationToken cancellationToken) =>
                {
                    if (!Guid.TryParse(req.RouteValues["categoryId"] as string, out Guid categoryId))
                    {
                        return Results.Problem("Category.Delete", "Invalid category ID format.");
                    }

                    var command = new DeleteCategoryCommand(categoryId);

                    var result = await sender.Send(command);

                    if (result.IsFailure)
                    {
                        return Results.Problem(result.Error);
                    }

                    return Results.Ok();
                })
                .WithName("DeleteCategory")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete Category")
                .WithDescription("Delete a category by ID.");
        }
    }
    public record DeleteCategoryCommand(Guid CategoryId) : ICommand<bool>;
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return Result.Failure<bool>(new Error("Category.NotFound", $"Category with ID {request.CategoryId} not found."));
            }

            await _categoryRepository.DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
    }

}
