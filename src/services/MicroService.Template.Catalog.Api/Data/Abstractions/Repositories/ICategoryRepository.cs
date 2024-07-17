using MicroService.Template.Catalog.Api.Models.Categories;

namespace MicroService.Template.Catalog.Api.Data.Abstractions.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid categoryId);
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
    }
}
