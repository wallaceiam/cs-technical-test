using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Events
{
    public class CashCardToppedUp : IEvent
    {
        public CashCardToppedUp(Guid cashCardId, decimal amountToppedUpBy, int version)
        {
            this.CashCardId = cashCardId;
            this.AmountToppedUpBy = amountToppedUpBy;
            this.SetVersion(version);
        }
        public int Version { get; private set; }
        public Guid CashCardId { get; }
        public decimal AmountToppedUpBy { get; }

        public void SetVersion(int version) => this.Version = version;
    }
}
