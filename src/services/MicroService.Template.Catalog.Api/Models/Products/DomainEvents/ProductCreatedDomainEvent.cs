using BuildingBlocks.Primitives;
using MicroService.Template.Catalog.Api.Models.Products;

namespace MicroService.Template.Catalog.Api.Models.Products.DomainEvents
{
    public sealed record ProductCreatedDomainEvent(Product Product) : DomainEvent(Guid.NewGuid());
}
