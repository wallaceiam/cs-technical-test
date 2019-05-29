using CreditSuisse.VirtualCashCard.Console.Interfaces;
using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.Commands;
using CreditSuisse.VirtualCashCard.Doman.Exceptions;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditSuisse.VirtualCashCard.Console.Services
{
    public class VirtualCashCardService : ICashCardService
    {
        private readonly ICommandBus sender;
        private readonly IEventStore eventStore;
        private readonly ILogger<VirtualCashCardService> logger;
        public VirtualCashCardService(ICommandBus sender, IEventStore eventStore, ILogger<VirtualCashCardService> logger)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Guid Create(string pin)
        {
            this.logger.LogInformation("Creating New Virtual Cash Card");

            try
            {
                var id = Guid.NewGuid();
                this.sender.Send(new CreateCashCard(id, pin));
                return id;
            }
            catch (InvalidPinException)
            {
                this.logger.LogError("Invalid Pin");
            }

            return Guid.Empty;
        }

        public decimal GetBalance(Guid id)
        {
            this.logger.LogInformation($"Getting Balance For Virtual Cash Card {id}");

            try
            {
                var cashCard = this.eventStore.GetById<CashCard>(id);
                return cashCard?.Balance ?? 0M;
            }
            catch (AggregateNotFoundException)
            {
                this.logger.LogError("No Cash Card With That Id");
            }
            return 0M;
        }

        public void InterCardTransfer(Guid cardA, string pin, Guid cardB, decimal amountToTransfer)
        {
            this.logger.LogInformation("Performing an inter cash card transfer");

            this.Withdraw(cardA, pin, amountToTransfer);
            try
            {
                this.TopUp(cardB, amountToTransfer);
            }
            catch (Exception ex)
            {
                this.logger.LogError("Reverting the transfer");
                this.TopUp(cardA, amountToTransfer);
            }
        }

        public void TopUp(Guid id, decimal amountToTopUpBy)
        {
            this.logger.LogInformation($"Topping Up {id} by {amountToTopUpBy:C}");

            try
            {
                this.sender.Send(new TopUpCashCard(id, amountToTopUpBy));
            }
            catch (InvalidAmountException)
            {
                this.logger.LogError("Invalid Amount");
            }
        }

        public void Withdraw(Guid id, string pin, decimal amountToWithDrawBy)
        {
            this.logger.LogInformation($"Withdrawing From {id} by {amountToWithDrawBy:C}");

            try
            {
                this.sender.Send(new WithdrawFromCashCard(id, pin, amountToWithDrawBy));
            }
            catch (InvalidPinException)
            {
                this.logger.LogError("Invalid Pin");
            }
            catch (IncorrectPinException)
            {
                this.logger.LogError("Incorrect Pin");
            }
            catch (InvalidAmountException)
            {
                this.logger.LogError("Invalid Amount");
            }
            catch (InsufficientFundsExceptions)
            {
                this.logger.LogError("Insufficient Funds");
            }

        }
    }
}
