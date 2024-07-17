using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace MicroService.Template.Catalog.Api.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await _dbContext.Set<Product>()
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Set<Product>()
                .Include(p => p.Categories)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _dbContext.Set<Product>()
                .Include(p => p.Categories)
                .Where(p => p.Categories.Any(c => c.Id == categoryId))
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _dbContext.Set<Product>().AddAsync(product);
        }

        public async Task UpdateAsync(Product product)
        {
            _dbContext.Set<Product>().Update(product);
        }

        public async Task DeleteAsync(Product product)
        {
            _dbContext.Set<Product>().Remove(product);
        }
    }
}
