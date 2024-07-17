using MicroService.Template.MongoDb.Abstractions;
using MicroService.Template.MongoDb.Constants;
using BuildingBlocks.OutBox.Models;
using BuildingBlocks.Primitives;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Template.MongoDb.Outbox
{
    public class MongoDbOutboxStore : IOutboxStore
    {
        private readonly IMongoCollection<OutboxMessage> _outboxMessages;

        public MongoDbOutboxStore(IOptions<MongoDbOutboxOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);

            var collectionName = options.Value.CollectionName;
            var collectionExists = database.ListCollectionNames().ToList().Contains(collectionName);
            if (!collectionExists)
            {
                database.CreateCollection(collectionName);
            }

            _outboxMessages = database.GetCollection<OutboxMessage>(collectionName);
        }
        public async Task Add<T>(T domainEvent) where T : IDomainEvent
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            };

            await _outboxMessages.InsertOneAsync(outboxMessage);
        }
    }
}
