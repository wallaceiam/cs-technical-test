using CreditSuisse.VirtualCashCard.Doman.Events;
using CreditSuisse.VirtualCashCard.Doman.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.AggregateRoots
{
    public class CashCard : AggregateRoot
    {
        private Guid id;
        private string pin;

        public CashCard() { }
        public CashCard(Guid id, string pin)
        {
            this.id = id;
            AssertPin(pin);
            this.ApplyChange(new CashCardCreated(id, pin, this.Version));
        }

        public override Guid Id => id;
        public decimal Balance { get; private set; }

        public void TopUp(decimal amountToTopUp)
        {
            AssertAmount(amountToTopUp);
            this.ApplyChange(new CashCardToppedUp(this.Id, amountToTopUp, this.Version));
        }

        public void Withdraw(string pin, decimal amountToWithdraw)
        {
            AssertPin(pin);
            AssertPinMatches(pin);
            AssertAmount(amountToWithdraw);
            AssertSufficentFunds(amountToWithdraw);
            this.ApplyChange(new CashCardWithdrawn(this.Id, amountToWithdraw, this.Version));
        }

        #region Validation

        private void AssertPin(string pin)
        {
            if(string.IsNullOrWhiteSpace(pin))
            {
                throw new InvalidPinException();
            }
            if(pin.Length != 4) // very arbitray
            {
                throw new InvalidPinException();
            }
            if(pin.Any(c => !char.IsDigit(c)))
            {
                throw new InvalidPinException();
            }
        }

        private void AssertPinMatches(string pin)
        {
            if(this.pin != pin)
            {
               throw new IncorrectPinException();
            }
        }

        private void AssertAmount(decimal amount)
        {
            if(amount <= 0)
            {
                throw new InvalidAmountException();
            }
            if(decimal.Round(amount, 2) != amount)
            {
                throw new InvalidAmountException();
            }
        }

        private void AssertSufficentFunds(decimal amount)
        {
            if(amount > this.Balance)
            {
                throw new InsufficientFundsExceptions();
            }
        }

        #endregion

        #region Apply Events
        private void Apply(CashCardCreated @event)
        {
            this.Version = @event.Version;
            this.id = @event.CashCardId;
            this.pin = @event.Pin;
            this.Balance = 0M;
        }

        private void Apply(CashCardToppedUp @event)
        {
            this.Version = @event.Version;
            this.Balance += @event.AmountToppedUpBy;
        }

        private void Apply(CashCardWithdrawn @event)
        {
            this.Version = @event.Version;
            this.Balance -= @event.AmountWithdrawnBy;
        }

        #endregion
    }
}
