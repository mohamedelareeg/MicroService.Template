using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Categories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MicroService.Template.Catalog.Api.Data.Repositories
{
    public class CachedCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryRepository _decorated;
        private readonly IDistributedCache _cache;

        public CachedCategoryRepository(ICategoryRepository decorated, IDistributedCache cache)
        {
            _decorated = decorated;
            _cache = cache;
        }

        private async Task<T?> GetOrSetAsync<T>(string cacheKey, Func<Task<T?>> factory, CancellationToken cancellationToken = default)
        {
            var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);
            if (!string.IsNullOrEmpty(cachedData))
            {
                return JsonConvert.DeserializeObject<T>(cachedData);
            }

            var result = await factory();
            if (result != null)
            {
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(result), cancellationToken);
            }

            return result;
        }

        private async Task ClearCache(Guid categoryId, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync($"category-{categoryId}", cancellationToken);
            await _cache.RemoveAsync("categories", cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(Guid categoryId)
        {
            string cacheKey = $"category-{categoryId}";
            return await GetOrSetAsync(cacheKey, () => _decorated.GetByIdAsync(categoryId));
        }

        public async Task<List<Category>> GetAllAsync()
        {
            string cacheKey = "categories";
            return await GetOrSetAsync(cacheKey, () => _decorated.GetAllAsync()) ?? new List<Category>();
        }

        public async Task AddAsync(Category category)
        {
            await _decorated.AddAsync(category);
            await ClearCache(category.Id);
        }

        public async Task UpdateAsync(Category category)
        {
            await _decorated.UpdateAsync(category);
            await ClearCache(category.Id);
        }

        public async Task DeleteAsync(Category category)
        {
            await _decorated.DeleteAsync(category);
            await ClearCache(category.Id);
        }
    }
}
