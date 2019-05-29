using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.Commands
{
    public class WithdrawFromCashCard : ICommand
    {
        public WithdrawFromCashCard(Guid cashCardId, string pin, decimal amountToWithdaw)
        {
            this.CashCardId = cashCardId;
            this.Pin = pin;
            this.AmountToWithdraw = amountToWithdaw;
        }
        public Guid CashCardId { get; }
        public decimal AmountToWithdraw { get; }
        public string Pin { get; }
    }
}
