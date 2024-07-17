using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using Carter.OpenApi;
using Mapster;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Categories;
using MicroService.Template.PostgreSQL.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Template.Catalog.Api.Features.Categories.Commands
{
    public class CreateCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/categories",
                async ([FromBody] CreateCategoryCommand request, ISender sender) =>
                {
                    var command = request.Adapt<CreateCategoryCommand>();

                    var result = await sender.Send(command);

                    if (result.IsFailure)
                    {
                        return Results.Problem(result.Error);
                    }

                    var response = new CreateCategoryResponse(result.Value.Id);
                    return Results.Created($"/categories/{response.Id}", response);
                })
                .WithName("CreateCategory")
                .Produces<CreateCategoryResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Category")
                .WithDescription("Create Category").IncludeInOpenApi();
        }
    }

    public record CreateCategoryResponse(Guid Id);
    public record CreateCategoryCommand(string Name) : ICommand<Category>;
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Category>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryResult = Category.Create(request.Name);
            if (categoryResult.IsFailure)
            {
                return Result.Failure<Category>(categoryResult.Error);
            }

            await _categoryRepository.AddAsync(categoryResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(categoryResult.Value);
        }
    }

}
