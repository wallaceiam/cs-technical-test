using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Console.Infrastructure
{
    public class AggregateEvent
    {
        public AggregateEvent(Guid aggregateId, IEvent eventData, int version)
        {
            this.EventData = eventData;
            this.Version = version;
            this.AggregateId = aggregateId;
        }

        public IEvent EventData { get; }
        public Guid AggregateId { get; }
        public int Version { get; }
    }
}
