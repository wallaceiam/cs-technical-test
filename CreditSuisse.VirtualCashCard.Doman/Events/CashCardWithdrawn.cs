using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Events
{
    public class CashCardWithdrawn : IEvent
    {
        public CashCardWithdrawn(Guid cashCardId, decimal amountWithdrawnBy, int version)
        {
            this.CashCardId = cashCardId;
            this.AmountWithdrawnBy = amountWithdrawnBy;
            this.SetVersion(version);
        }
        public int Version { get; private set; }
        public Guid CashCardId { get; }
        public decimal AmountWithdrawnBy { get; }

        public void SetVersion(int version) => this.Version = version;
    }
}
