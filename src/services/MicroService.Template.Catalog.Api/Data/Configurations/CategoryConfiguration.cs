using MicroService.Template.Catalog.Api.Models.Categories;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MicroService.Template.Catalog.Api.Data.Configurations
{
    internal sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(TableNames.Categories);


            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired();

        }
    }
}
