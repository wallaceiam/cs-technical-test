using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Commands
{
    public class TopUpCashCard : ICommand
    {
        public TopUpCashCard(Guid cashCardId, decimal amountToTopUp)
        {
            this.CashCardId = cashCardId;
            this.AmountToTopUp = amountToTopUp;
        }
        public Guid CashCardId { get; }
        public decimal AmountToTopUp { get; }
    }
}
