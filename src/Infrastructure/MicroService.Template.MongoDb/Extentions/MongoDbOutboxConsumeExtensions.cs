using MicroService.Template.MongoDb.Outbox;
using BuildingBlocks.OutBox.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Quartz;

namespace MicroService.Template.MongoDb.Extentions
{
    public static class MongoDbOutboxConsumeExtensions
    {
        public static IServiceCollection AddMongoDbOutboxConsume(this IServiceCollection services, IConfiguration configuration)
        {
            // Quartz
            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(MongoDbOutboxProcessMessagesJob));
                configure.AddJob<MongoDbOutboxProcessMessagesJob>(jobKey).AddTrigger(trigger =>
                trigger.ForJob(jobKey).WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(10).RepeatForever()));
                configure.UseMicrosoftDependencyInjectionJobFactory();
            });
            services.AddQuartzHostedService();

            return services;
        }
    }
}
