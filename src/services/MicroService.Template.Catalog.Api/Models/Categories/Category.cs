using BuildingBlocks.Domain.Shared.Guards;
using BuildingBlocks.Primitives;
using BuildingBlocks.Results;
using MicroService.Template.Catalog.Api.Models.Categories.DomainEvents;

namespace MicroService.Template.Catalog.Api.Models.Categories
{
    public sealed class Category : AggregateRoot , IAuditableEntity
    {
        private Category() { }

        public Category(Guid id, string name)
        {
            Guard.Against.InValidGuid(id.ToString(), nameof(id));
            Guard.Against.NullOrEmpty(name, nameof(name));

            Id = id;
            Name = name;

            RaiseDomainEvent(new CategoryCreatedDomainEvent(this));
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;

        public static Result<Category> Create(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result.Failure<Category>("Category.Create", "Category name cannot be empty.");
                }

                var category = new Category(Guid.NewGuid(), name);
                return Result.Success(category);
            }
            catch (Exception ex)
            {
                return Result.Failure<Category>(new Error("Category.Create", $"Failed to create Category: {ex.Message}"));
            }
        }
    }
}
