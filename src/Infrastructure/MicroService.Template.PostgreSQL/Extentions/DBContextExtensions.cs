using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MicroService.Template.PostgreSQL.Abstractions;
namespace MicroService.Template.PostgreSQL.Extentions
{
    public static class DBContextExtensions
    {
        public static IServiceCollection AddPostgresDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((sp, options) =>
            //{
            //    ConvertDomainEventsToOutboxMessagesInterceptor interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            //    string connectionString = configuration.GetConnectionString("DBConnection");
            //    options.UseSqlServer(connectionString)
            //           .AddInterceptors(interceptor);
            //});
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
