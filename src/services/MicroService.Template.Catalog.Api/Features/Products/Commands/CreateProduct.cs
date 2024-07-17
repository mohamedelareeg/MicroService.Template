using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using Mapster;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Features.Categories.Commands;
using MicroService.Template.Catalog.Api.Models.Products;
using MicroService.Template.PostgreSQL.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Template.Catalog.Api.Features.Products.Commands
{
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products",
                async ([FromBody] CreateCategoryResponse request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = request.Adapt<CreateProductCommand>();

                    var result = await sender.Send(command);

                    return result.IsSuccess ? Results.Created("/products/" + result.Value.Id, result.Value) : Results.Problem(result.Error);
                })
                .Produces<Product>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("CreateProduct")
                .WithSummary("Create Product")
                .WithDescription("Create a new product.");
        }
    }
    public record CreateProductCommand(string Name, string Description, string ImageFile, decimal Price) : ICommand<Product>;

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productResult = Product.Create(request.Name, request.Description, request.ImageFile, request.Price);
            if (productResult.IsFailure)
            {
                return Result.Failure<Product>(productResult.Error);
            }

            await _productRepository.AddAsync(productResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(productResult.Value);
        }
    }
}
