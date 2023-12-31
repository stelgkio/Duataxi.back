﻿using Chronicle;
using DuaTaxi.Common;
using DuaTaxi.Common.RabbitMq;
using System;
using System.Collections.Generic;

namespace DuaTaxi.Services.Operations.Sagas
{
    public class SagaContext : ISagaContext
    {
        public Guid CorrelationId { get; }

        public string Originator { get; }
        public IReadOnlyCollection<ISagaContextMetadata> Metadata { get; }

        private SagaContext(Guid correlationId, string originator)
            => (CorrelationId, Originator) = (correlationId, originator);

        public static ISagaContext Empty
            => new SagaContext(Guid.Empty, string.Empty);

        public static ISagaContext FromCorrelationContext(ICorrelationContext context)
            => new SagaContext(context.Id.ToGuid(), context.Resource);
        
        public ISagaContextMetadata GetMetadata(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetMetadata(string key, out ISagaContextMetadata metadata)
        {
            throw new NotImplementedException();
        }
    }
}
