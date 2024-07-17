using MicroService.Template.MongoDb.Abstractions;
using MicroService.Template.MongoDb.Constants;
using MicroService.Template.MongoDb.Outbox;
using BuildingBlocks.OutBox.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroService.Template.MongoDb.Extentions
{
    public static class MongoDbOutboxExtensions
    {
        public static IServiceCollection AddMongoDbOutbox(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new MongoDbOutboxOptions();
            configuration.GetSection(nameof(OutboxOptions)).Bind(options);
            services.Configure<MongoDbOutboxOptions>(configuration.GetSection(nameof(OutboxOptions)));

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var mongoOptions = serviceProvider.GetRequiredService<IOptions<MongoDbOutboxOptions>>().Value;
                return new MongoClient(mongoOptions.ConnectionString);
            });

            services.AddSingleton(serviceProvider =>
            {
                var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
                var mongoOptions = serviceProvider.GetRequiredService<IOptions<MongoDbOutboxOptions>>().Value;
                var database = mongoClient.GetDatabase(mongoOptions.DatabaseName);
                return database.GetCollection<OutboxMessage>(mongoOptions.CollectionName);
            });

            services.AddScoped<IOutboxStore, MongoDbOutboxStore>();

            return services;
        }
    }
}
