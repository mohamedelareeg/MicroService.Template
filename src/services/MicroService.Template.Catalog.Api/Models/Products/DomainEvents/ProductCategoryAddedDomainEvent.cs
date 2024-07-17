using BuildingBlocks.Primitives;

namespace MicroService.Template.Catalog.Api.Models.Products.DomainEvents
{
    public sealed record ProductCategoryAddedDomainEvent(Guid ProductId, Guid CategoryId) : DomainEvent(Guid.NewGuid());
}
