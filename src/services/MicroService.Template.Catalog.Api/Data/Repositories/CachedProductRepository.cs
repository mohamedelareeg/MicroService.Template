using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Models.Products;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MicroService.Template.Catalog.Api.Data.Repositories
{
    public class CachedProductRepository : IProductRepository
    {
        private readonly IProductRepository _decorated;
        private readonly IDistributedCache _cache;

        public CachedProductRepository(IProductRepository decorated, IDistributedCache cache)
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

        private async Task ClearCache(Guid productId, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync($"product-{productId}", cancellationToken);
            await _cache.RemoveAsync("products", cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            string cacheKey = $"product-{productId}";
            return await GetOrSetAsync(cacheKey, () => _decorated.GetByIdAsync(productId));
        }

        public async Task<List<Product>> GetAllAsync()
        {
            string cacheKey = "products";
            return await GetOrSetAsync(cacheKey, () => _decorated.GetAllAsync()) ?? new List<Product>();
        }

        public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId)
        {
            string cacheKey = $"products-by-category-{categoryId}";
            return await GetOrSetAsync(cacheKey, () => _decorated.GetByCategoryIdAsync(categoryId)) ?? new List<Product>();
        }

        public async Task AddAsync(Product product)
        {
            await _decorated.AddAsync(product);
            await ClearCache(product.Id);
        }

        public async Task UpdateAsync(Product product)
        {
            await _decorated.UpdateAsync(product);
            await ClearCache(product.Id);
        }

        public async Task DeleteAsync(Product product)
        {
            await _decorated.DeleteAsync(product);
            await ClearCache(product.Id);
        }
    }
}
