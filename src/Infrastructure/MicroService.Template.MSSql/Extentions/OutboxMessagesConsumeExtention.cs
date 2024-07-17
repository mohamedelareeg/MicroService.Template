using MicroService.Template.MSSql.BackgroundJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Template.MSSql.Extentions
{
    public static class OutboxMessagesConsumeExtention
    {
        public static IServiceCollection AddOutboxMessagesConsumeDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Quartz
            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
                configure.AddJob<ProcessOutboxMessagesJob>(jobKey).AddTrigger(trigger =>
                trigger.ForJob(jobKey).WithSimpleSchedule(schedule =>
                schedule.WithIntervalInSeconds(10).RepeatForever()));
                configure.UseMicrosoftDependencyInjectionJobFactory();
            });
            services.AddQuartzHostedService();
            return services;
        }
    }
}
