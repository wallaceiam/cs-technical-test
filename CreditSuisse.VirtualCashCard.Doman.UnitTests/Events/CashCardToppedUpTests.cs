using System;
using Xunit;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.Events;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.Events
{
    public class CashCardToppedUpTests
    {
        [Fact]
        public void CashCardToppedUpTests_ShouldCreate()
        {
            var expectedId = Guid.NewGuid();
            var expectedAmount = 56.78M;
            var expectedVersion = 3;

            var @event = new CashCardToppedUp(expectedId, expectedAmount, expectedVersion);

            @event.CashCardId.Should().Be(expectedId);
            @event.AmountToppedUpBy.Should().Be(expectedAmount);
            @event.Version.Should().Be(expectedVersion);
        }
    }
}
