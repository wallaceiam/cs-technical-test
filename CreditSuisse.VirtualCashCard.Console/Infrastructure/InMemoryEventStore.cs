using CreditSuisse.VirtualCashCard.Doman;
using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.Exceptions;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Console.Infrastructure
{
    public class InMemoryEventStore : IEventStore
    {
        public InMemoryEventStore()
        {
        }

        private static readonly Dictionary<Guid, List<AggregateEvent>> allEvents = new Dictionary<Guid, List<AggregateEvent>>();

        public T GetById<T>(Guid id) where T: AggregateRoot, new()
        {
            var aggreage = new T();
            var e = this.GetEventsForAggregate(id);
            aggreage.LoadEventsFromHistory(e);
            return aggreage;
        } 

        public void Save<T>(T aggregate) where T: AggregateRoot
        {
            this.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }

        private void SaveEvents(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion)
        {
            
            if (!allEvents.TryGetValue(aggregateId, out var eventDescriptors))
            {
                eventDescriptors = new List<AggregateEvent>();
                allEvents.Add(aggregateId, eventDescriptors);
            }
            else if (eventDescriptors.Last().Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }

            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;
                @event.SetVersion(version);

                eventDescriptors.Add(new AggregateEvent(aggregateId, @event, version));                
            }
        }

        private IEnumerable<IEvent> GetEventsForAggregate(Guid aggregateId)
        {
            if (!allEvents.TryGetValue(aggregateId, out var eventDescriptors))
            {
                throw new AggregateNotFoundException();
            }

            return eventDescriptors.Select(desc => desc.EventData);
        }
    }
}
