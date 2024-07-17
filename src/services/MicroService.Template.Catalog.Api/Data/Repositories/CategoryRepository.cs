using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Categories;
using Microsoft.EntityFrameworkCore;

namespace MicroService.Template.Catalog.Api.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            return await _dbContext.Set<Category>().FindAsync(categoryId);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbContext.Set<Category>().ToListAsync();
        }

        public async Task AddAsync(Category category)
        {
            await _dbContext.Set<Category>().AddAsync(category);
        }

        public async Task UpdateAsync(Category category)
        {
            _dbContext.Set<Category>().Update(category);
        }

        public async Task DeleteAsync(Category category)
        {
            _dbContext.Set<Category>().Remove(category);
        }
    }
}
