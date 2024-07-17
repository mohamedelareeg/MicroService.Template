using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MicroService.Template.PostgreSQL;
using MicroService.Template.Catalog.Api.Data;
using MicroService.Template.PostgreSQL.Interceptors;
using Microsoft.EntityFrameworkCore;
using MicroService.Template.PostgreSQL.Extentions;
using MediatR;
using BuildingBlocks.Behaviors;
using FluentValidation;
using MicroService.Template.PostgreSQL.Idempotence;
using MicroService.Template.Redis.Extentions;
using MicroService.Template.Catalog.Api.Data.Abstractions.Repositories;
using MicroService.Template.Catalog.Api.Data.Repositories;
using Carter;
namespace ARPSolutions.Accounting.Core.Extentions
{
    public static class Dependencies
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) =>
            {
                ConvertDomainEventsToOutboxMessagesInterceptor interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
                string connectionString = configuration.GetConnectionString("DBConnection");
                options.UseNpgsql(connectionString)
                       .AddInterceptors(interceptor);
            });
            #region Mediator & Pipelines
            //MediatR
            services.AddMediatR(cfg =>
            {                                                                                                                                                                                               
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                //MediatR PopleLines
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationPiplineBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            });

            //Fluent Validation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
            #endregion
            services.AddPostgresDependencies(configuration);
            services.AddOutboxMessagesConsumeDependencies(configuration);

            //services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
            #region AutoMapper
            // Mapping Profiles

            #endregion
            #region Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            #endregion
            #region Repositories Caching
            services.Decorate<IProductRepository, CachedProductRepository>();
            services.Decorate<ICategoryRepository, CachedCategoryRepository>();
            services.AddRedisDependencies(configuration);
            #endregion
            return services;
        }
    }
}
