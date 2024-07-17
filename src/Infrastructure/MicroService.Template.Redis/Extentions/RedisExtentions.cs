using MicroService.Template.Redis.Abstractions;
using MicroService.Template.Redis.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Template.Redis.Extentions
{
    public static class RedisExtentions
    {
        public static IServiceCollection AddRedisDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(redisOptions => {
                string connectionString = configuration.GetConnectionString("Redis");
                redisOptions.Configuration = connectionString;
            });
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}
