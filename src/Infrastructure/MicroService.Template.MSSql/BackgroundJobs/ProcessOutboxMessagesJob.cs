using BuildingBlocks.OutBox.Models;
using BuildingBlocks.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Template.MSSql.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private readonly IApplicationDbContext _context;
        private readonly IPublisher _publisher;

        public ProcessOutboxMessagesJob(IApplicationDbContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        private static readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        // Idempotency >> Certain Operation Multiple times without change intial result
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                List<OutboxMessage> messages = await _context.Set<OutboxMessage>()//TODO FIX IT
                    .Where(a => a.ProcessedOnUtc == null)
                    .Take(20)
                    .ToListAsync(context.CancellationToken);

                foreach (OutboxMessage? message in messages)
                {
                    IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        message.Content,
                        jsonSerializerSettings);

                    if (domainEvent is null)
                    {
                        // TODO: Logging
                        continue;
                    }

                    AsyncRetryPolicy policy = Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(
                            3,
                            attempt => TimeSpan.FromMilliseconds(50 * attempt));

                    PolicyResult result = await policy.ExecuteAndCaptureAsync(() =>
                        _publisher.Publish(
                            domainEvent,
                            context.CancellationToken));

                    message.Error = result.FinalException?.ToString();
                    message.ProcessedOnUtc = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO: Logging
                // Handle specific exceptions like violation of primary key constraint
            }
        }
    }
}
