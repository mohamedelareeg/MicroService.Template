using MicroService.Template.MSSql.Idempotence;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Template.MSSql.Extentions
{
    public static class OutboxMessagesDomainEventsConsumeExtension
    {
        public static IServiceCollection AddOutboxMessagesDomainEventsConsumeDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));
            return services;
        }
       
    }
}
