using BuildingBlocks.OutBox.Models;
using BuildingBlocks.Primitives;
using MediatR;
using MongoDB.Driver;
using Newtonsoft.Json;
using Polly.Retry;
using Polly;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Template.MongoDb.Outbox
{
    public class MongoDbOutboxProcessMessagesJob : IJob
    {
        private readonly IMongoCollection<OutboxMessage> _outboxCollection;
        private readonly IPublisher _publisher;

        private static readonly JsonSerializerSettings jsonSerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
        };

        public MongoDbOutboxProcessMessagesJob(IMongoClient mongoClient, IPublisher publisher, string databaseName, string collectionName)
        {
            var database = mongoClient.GetDatabase(databaseName);
            _outboxCollection = database.GetCollection<OutboxMessage>(collectionName);
            _publisher = publisher;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var filter = Builders<OutboxMessage>.Filter.Eq(m => m.ProcessedOnUtc, null);
                var messages = await _outboxCollection.Find(filter).Limit(20).ToListAsync(context.CancellationToken);

                foreach (var message in messages)
                {
                    IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content, jsonSerializerSettings);
                    if (domainEvent == null)
                    {
                        // TODO: Logging
                        continue;
                    }

                    AsyncRetryPolicy policy = Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(50 * attempt));

                    PolicyResult result = await policy.ExecuteAndCaptureAsync(() => _publisher.Publish(domainEvent, context.CancellationToken));

                    var update = Builders<OutboxMessage>.Update
                        .Set(m => m.Error, result.FinalException?.ToString())
                        .Set(m => m.ProcessedOnUtc, DateTime.UtcNow);

                    await _outboxCollection.UpdateOneAsync(Builders<OutboxMessage>.Filter.Eq(m => m.Id, message.Id), update, cancellationToken: context.CancellationToken);
                }
            }
            catch (Exception ex)
            {
                // TODO: Logging
            }
        }
    }
}