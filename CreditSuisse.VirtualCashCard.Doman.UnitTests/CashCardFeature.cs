using System;
using Xunit;
using Xbehave;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.Events;
using System.Linq;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using CreditSuisse.VirtualCashCard.Doman.Exceptions;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests
{
    // In order to build complicated virtual cash cards
    // As a professional software developer with excellent skills
    // I want a cash card top be able to perform basic top up and withdraws
    public class CashCardFeature
    {
        [Scenario]
        public void CashCard_ShouldCreate(Guid id, CashCard cashCard)
        {
            "Given there is no cash card"
               .x(() => { id = Guid.NewGuid(); });
            "When I create a cash card"
                .x(() => cashCard = new CashCard(id, "1234"));
            "Then the cash card should raise a created event"
                .x(() =>
                {
                    cashCard.GetUncommittedChanges().Count().Should().Be(1);
                    var actualEvent = cashCard.GetUncommittedChanges().First();

                    var @event = new CashCardCreated(id, "1234", 0);
                    actualEvent.Should().BeEquivalentTo(@event);
                });
        }

        [Scenario]
        public void CashCard_ShouldTopUp(Guid id, CashCard cashCard)
        {
            "Given there is a cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new[] { new CashCardCreated(id, "1234", 0) });
                });
            "When I top up £50"
                .x(() => cashCard.TopUp(50.00M));
            "Then the cash card should raise a top up event"
                .x(() =>
                {
                    cashCard.GetUncommittedChanges().Count().Should().Be(1);
                    var actualEvent = cashCard.GetUncommittedChanges().First();

                    var @event = new CashCardToppedUp(id, 50.00M, 0);
                    actualEvent.Should().BeEquivalentTo(@event);
                });
        }

        [Scenario]
        public void CashCard_ShouldWithdraw(Guid id, CashCard cashCard)
        {
            "Given there is a cash card with a £50 balance"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new IEvent[] {
                        new CashCardCreated(id, "1234", 0),
                        new CashCardToppedUp(id, 50.00M, 1)
                    });
                });
            "When I with draw £50"
                .x(() => cashCard.Withdraw("1234", 50.00M));
            "Then the cash card should raise a with draw event"
                .x(() =>
                {
                    cashCard.GetUncommittedChanges().Count().Should().Be(1);
                    var actualEvent = cashCard.GetUncommittedChanges().First();

                    var @event = new CashCardWithdrawn(id, 50.00M, 1);
                    actualEvent.Should().BeEquivalentTo(@event);
                });
        }

        [Scenario]
        [Example("abcd")]
        [Example("1a23")]
        [Example("1")]
        [Example("123")]
        [Example("12345")]
        [Example("abced")]
        [Example("")]
        [Example(null)]
        public void CashCard_ShouldNotCreate_IfPinIsInvalid(string pin, Guid id, CashCard cashCard)
        {
            "Given there is no cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                });
            $"When I create one with an invalid pin {pin}"
                .x(() => Assert.Throws<InvalidPinException>(() => cashCard = new CashCard(id, pin)));
            "Then an InvalidPinException is thrown".x(() => { });
        }

        [Scenario]
        [Example(-1)]
        [Example(0)]
        [Example(0.001)]
        public void CashCard_ShouldNotTopUp_IfAmountIsInvalid(decimal amount, Guid id, CashCard cashCard)
        {
            "Given there is a cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new[] { new CashCardCreated(id, "1234", 1) });
                });
            $"When I top up with an invalid amount {amount}"
                .x(() => Assert.Throws<InvalidAmountException>(() => cashCard.TopUp(amount)));
            "Then an InvalidAmountException is thrown".x(() => { });
        }

        [Scenario]
        [Example(-1)]
        [Example(0)]
        [Example(0.001)]
        public void CashCard_ShouldNotWithdraw_IfAmountIsInvalid(decimal amount, Guid id, CashCard cashCard)
        {
            "Given there is a cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new IEvent[] {
                        new CashCardCreated(id, "1234", 1),
                        new CashCardToppedUp(id, 100.00M, 2)
                    });
                });
            $"When I withdraw with an invalid amount {amount}"
                .x(() => Assert.Throws<InvalidAmountException>(() => cashCard.Withdraw("1234", amount)));
            "Then an InvalidAmountException is thrown".x(() => { });
        }

        [Scenario]
        [Example("abcd")]
        [Example("1a23")]
        [Example("1")]
        [Example("123")]
        [Example("12345")]
        [Example("abced")]
        [Example("")]
        [Example(null)]
        public void CashCard_ShouldNotWithdraw_IfPinIsInvalid(string pin, Guid id, CashCard cashCard)
        {
            "Given there is a cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new IEvent[] {
                        new CashCardCreated(id, "1234", 1),
                        new CashCardToppedUp(id, 100.00M, 2)
                    });
                });
            $"When I withdraw with an invalid pin {pin}"
                .x(() => Assert.Throws<InvalidPinException>(() => cashCard.Withdraw(pin, 10.00M)));
            "Then an InvalidPinException is thrown".x(() => { });
        }

        [Scenario]
        public void CashCard_ShouldNotWithdraw_IfPinDoesNotMatch(Guid id, CashCard cashCard)
        {
            "Given there is a cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new IEvent[] {
                        new CashCardCreated(id, "1234", 1),
                        new CashCardToppedUp(id, 100.00M, 2)
                    });
                });
            "When I withdraw with a different pin"
                .x(() => Assert.Throws<IncorrectPinException>(() => cashCard.Withdraw("4321", 10.00M)));
            "Then an IncorrectPinException is thrown".x(() => { });
        }

        [Scenario]
        public void CashCard_ShouldNotWithdraw_IfInsufficientFunds(Guid id, CashCard cashCard)
        {
            "Given there is a cash card"
                .x(() =>
                {
                    id = Guid.NewGuid();
                    cashCard = new CashCard();
                    cashCard.LoadEventsFromHistory(new IEvent[] {
                        new CashCardCreated(id, "1234", 1),
                        new CashCardToppedUp(id, 100.00M, 2)
                    });
                });
            "When I withdraw with a different pin"
                .x(() => Assert.Throws<InsufficientFundsExceptions>(() => cashCard.Withdraw("1234", 100.01M)));
            "Then an InsufficientFundsExceptions is thrown".x(() => { });
        }

    }
}
