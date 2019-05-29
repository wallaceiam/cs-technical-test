using System;
using Xunit;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.Events;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.Events
{
    public class CashCardWithdrawnTests
    {
        [Fact]
        public void CashCardWithdrawn_ShouldCreate()
        {
            var expectedId = Guid.NewGuid();
            var expectedAmount = 56.78M;
            var expectedVersion = 4;

            var @event = new CashCardWithdrawn(expectedId, expectedAmount, expectedVersion);

            @event.CashCardId.Should().Be(expectedId);
            @event.AmountWithdrawnBy.Should().Be(expectedAmount);
            @event.Version.Should().Be(expectedVersion);
        }
    }
}
