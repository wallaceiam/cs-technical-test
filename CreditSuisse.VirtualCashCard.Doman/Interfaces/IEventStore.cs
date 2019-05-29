using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Interfaces
{
    public interface IEventStore
    {
        T GetById<T>(Guid id) where T : AggregateRoot, new();
        void Save<T>(T aggregate) where T : AggregateRoot;
    }
}
