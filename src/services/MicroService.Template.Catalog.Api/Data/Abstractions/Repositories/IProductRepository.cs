using MicroService.Template.Catalog.Api.Models.Products;

namespace MicroService.Template.Catalog.Api.Data.Abstractions.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid productId);
        Task<List<Product>> GetAllAsync();
        Task<List<Product>> GetByCategoryIdAsync(Guid categoryId);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
