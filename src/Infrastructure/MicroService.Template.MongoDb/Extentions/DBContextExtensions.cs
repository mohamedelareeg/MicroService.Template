using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MicroService.Template.MongoDb.Constants;
using MicroService.Template.MongoDb.Abstractions;

namespace MicroService.Template.MongoDb.Extentions
{
    public static class DBContextExtensions
    {
        public static IServiceCollection AddMongoDBDependencies(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.Configure<MongoDbOptions>(configuration.GetSection(nameof(MongoDbOptions)));
            services.AddScoped<IMongoDbContext, MongoDbContext>();

            // Register repositories
            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            return services;
        }
    }
}
