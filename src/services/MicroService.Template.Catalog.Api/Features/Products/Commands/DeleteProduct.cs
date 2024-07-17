using BuildingBlocks.Messaging;
using BuildingBlocks.Results;
using Carter;
using MediatR;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.PostgreSQL.Abstractions;

namespace MicroService.Template.Catalog.Api.Features.Products.Commands
{
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{productId}",
                async (Guid productId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new DeleteProductCommand(productId);

                    var result = await sender.Send(command);

                    return result.IsSuccess ? Results.Ok() : Results.Problem(result.Error);
                })
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithName("DeleteProduct")
                .WithSummary("Delete Product")
                .WithDescription("Delete an existing product by its ID.");
        }
    }
    public record DeleteProductCommand(Guid ProductId) : ICommand<bool>;
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result.Failure<bool>(new Error("Product.NotFound", $"Product with ID {request.ProductId} not found."));
            }

            await _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(true);
        }
    }

}
