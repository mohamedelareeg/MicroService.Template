using BuildingBlocks.OutBox.Models;
using BuildingBlocks.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.Template.MongoDb.Abstractions
{
    public interface IOutboxStore
    {
        Task Add<T>(T domainEvent) where T : IDomainEvent;
    }
}
