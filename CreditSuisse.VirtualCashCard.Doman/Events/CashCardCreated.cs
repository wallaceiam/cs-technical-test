using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Events
{
    public class CashCardCreated : IEvent
    {
        public CashCardCreated(Guid cashCardId, string pin, int version)
        {
            this.CashCardId = cashCardId;
            this.Pin = pin;
            this.SetVersion(version);
        }
        public int Version { get; private set; }
        public Guid CashCardId { get; }
        public string Pin { get; }

        public void SetVersion(int version) => this.Version = version;
    }
}
