using MicroService.Template.Catalog.Api.Models.Products;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MicroService.Template.Catalog.Api.ValueObjects;

namespace MicroService.Template.Catalog.Api.Data.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(TableNames.Products);

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.ImageFile)
                .IsRequired();

            builder.OwnsOne(p => p.Price, price =>
            {
                price.Property(p => p.Amount)
                    .HasColumnName(nameof(Price.Amount))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

            });

            builder.HasMany(p => p.Categories)
                .WithOne()
                .HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
