using BuildingBlocks.Primitives;

namespace MicroService.Template.Catalog.Api.Models.Categories.DomainEvents
{
    public sealed record CategoryCreatedDomainEvent(Category Category) : DomainEvent(Guid.NewGuid());
}
