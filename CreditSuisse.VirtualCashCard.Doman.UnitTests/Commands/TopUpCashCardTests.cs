using System;
using Xunit;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.Commands;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.Commands
{
    public class TopUpCashCardTests
    {
        [Fact]
        public void TopUpCashCard_ShouldCreate()
        {
            var expectedId = Guid.NewGuid();
            var expectedAmount = 56.78M;

            var command = new TopUpCashCard(expectedId, expectedAmount);

            command.CashCardId.Should().Be(expectedId);
            command.AmountToTopUp.Should().Be(expectedAmount);
        }
    }
}
