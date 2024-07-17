using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Products;
using MicroService.Template.PostgreSQL.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Template.Catalog.Api.Features.Products.Commands
{
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{productId}",
                async ([FromBody] UpdateProductCommand request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(request);

                    return result.IsSuccess ? Results.Ok() : Results.Problem(result.Error);
                })
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("UpdateProduct")
                .WithSummary("Update Product")
                .WithDescription("Update an existing product by its ID.");
        }
    }
    public record UpdateProductCommand(Guid ProductId, string Name, string Description, string ImageFile, decimal Price) : ICommand<Product>;
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<Product>(new Error("Product.NotFound", $"Product with ID {request.ProductId} not found."));
            }

            var updateResult = product.UpdateProduct(request.Name, request.Description, request.ImageFile, request.Price);
            if (updateResult.IsFailure)
            {
                return Result.Failure<Product>(updateResult.Error);
            }

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(product);
        }
    }

}
