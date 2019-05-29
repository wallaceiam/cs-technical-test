using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Commands
{
    public class CreateCashCard : ICommand
    {

        public CreateCashCard(Guid cashCardId, string pin)
        {
            this.CashCardId = cashCardId;
            this.Pin = pin;
        }

        public Guid CashCardId { get; }
        public string Pin { get; }
    }
}
