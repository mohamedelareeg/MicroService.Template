using System;
using System.Collections.Generic;
using BuildingBlocks.Domain.Shared.Guards;
using BuildingBlocks.Primitives;
using BuildingBlocks.Results;
using MicroService.Template.Catalog.Api.Models.Categories;
using MicroService.Template.Catalog.Api.Models.Products.DomainEvents;
using MicroService.Template.Catalog.Api.ValueObjects;

namespace MicroService.Template.Catalog.Api.Models.Products
{
    public sealed class Product : AggregateRoot, IAuditableEntity
    {
        private readonly List<Category> _categories = new();

        private Product() { }

        public Product(Guid id, string name, string description, string imageFile, Price price)
        {
            Guard.Against.InValidGuid(id.ToString(), nameof(id));
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NullOrEmpty(imageFile, nameof(imageFile));

            Id = id;
            Name = name;
            Description = description;
            ImageFile = imageFile;
            Price = price;

            RaiseDomainEvent(new ProductCreatedDomainEvent(this));
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public string ImageFile { get; private set; } = default!;
        public Price Price { get; private set; }
        public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

        public static Result<Product> Create(string name, string description, string imageFile, decimal price)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Result.Failure<Product>("Product.Create", "Product name cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(description))
                {
                    return Result.Failure<Product>("Product.Create", "Product description cannot be empty.");
                }

                if (string.IsNullOrWhiteSpace(imageFile))
                {
                    return Result.Failure<Product>("Product.Create", "Product image file cannot be empty.");
                }

                var priceResult = Price.Create(price);
                if (priceResult.IsFailure)
                {
                    return Result.Failure<Product>(priceResult.Error);
                }

                var product = new Product(Guid.NewGuid(), name, description, imageFile, priceResult.Value);
                return Result.Success(product);
            }
            catch (Exception ex)
            {
                return Result.Failure<Product>(new Error("Product.Create", $"Failed to create Product: {ex.Message}"));
            }
        }

        public Result<Product> AddCategory(Category category)
        {
            Guard.Against.Null(category, nameof(category));
            _categories.Add(category);
            RaiseDomainEvent(new ProductCategoryAddedDomainEvent(Id, category.Id));
            return Result.Success(this);
        }

        public Result<Product> UpdateProduct(string name, string description, string imageFile, decimal price)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NullOrEmpty(imageFile, nameof(imageFile));
            Guard.Against.NumberNegativeOrZero(price, nameof(price));

            Name = name;
            Description = description;
            ImageFile = imageFile;
            var priceResult = Price.Create(price);
            if (priceResult.IsFailure)
            {
                return Result.Failure<Product>(priceResult.Error);
            }
            Price = priceResult.Value;

            RaiseDomainEvent(new ProductUpdatedDomainEvent(this));
            return Result.Success(this);
        }
    }
}
