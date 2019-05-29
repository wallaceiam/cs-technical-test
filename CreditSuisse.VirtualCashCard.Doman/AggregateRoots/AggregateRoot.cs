using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.AggregateRoots
{
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> events = new List<IEvent>();

        public abstract Guid Id { get; }
        public int Version { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return events;
        }

        public void MarkChangesAsCommitted()
        {
            events.Clear();
        }

        public void LoadEventsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(IEvent @event, bool isNew = true)
        {
            var type = this.GetType();
            var method = type.GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance,
                null, new[] { @event.GetType() }, null);

            method.Invoke(this, new[] { @event });

            if (isNew) events.Add(@event);
        }
    }
}
