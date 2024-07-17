using BuildingBlocks.Messaging;
using BuildingBlocks.OutBox.Models;
using BuildingBlocks.Primitives;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Template.PostgreSQL.Idempotence
{
    public sealed class IdempotentDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
       where TDomainEvent : IDomainEvent
    {
        private readonly INotificationHandler<TDomainEvent> _decorated;
        private readonly IApplicationDbContext _context;

        public IdempotentDomainEventHandler(INotificationHandler<TDomainEvent> decorated, IApplicationDbContext context)
        {
            _decorated = decorated;
            _context = context;
        }

        public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
        {
            // Identify the consumer of the domain event handler
            string consumer = _decorated.GetType().Name;

            // Check if the domain event consumer has already processed this event
            bool exists = await _context.AnyAsync<OutboxMessageConsumer>(
                outboxMessageConsumer =>
                    outboxMessageConsumer.Id == notification.Id &&
                    outboxMessageConsumer.Name == consumer,
                cancellationToken);

            if (exists)
            {
                return; // If already processed, return without further action
            }

            // Handle the domain event using the decorated handler
            await _decorated.Handle(notification, cancellationToken);

            // Record the processed domain event in the outbox message consumers
            var outboxMessageConsumer = new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            };
            _context.Set<OutboxMessageConsumer>().Add(outboxMessageConsumer);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
