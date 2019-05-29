using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.Commands;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Doman.CommandHandlers
{
    public class CashCardCommandHandler : ICommandHandler<CreateCashCard>, ICommandHandler<TopUpCashCard>, ICommandHandler<WithdrawFromCashCard>
    {
        private readonly IEventStore eventStore;

        public CashCardCommandHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public void Handle(CreateCashCard command)
        {
            var cashCard = new CashCard(command.CashCardId, command.Pin);
            this.eventStore.Save(cashCard);
        }

        public void Handle(TopUpCashCard command)
        {
            var cashCard = this.eventStore.GetById<CashCard>(command.CashCardId);
            cashCard.TopUp(command.AmountToTopUp);
            this.eventStore.Save(cashCard);
        }

        public void Handle(WithdrawFromCashCard command)
        {
            var cashCard = this.eventStore.GetById<CashCard>(command.CashCardId); 
            cashCard.Withdraw(command.Pin, command.AmountToWithdraw);
            this.eventStore.Save(cashCard);
        }
    }
}
