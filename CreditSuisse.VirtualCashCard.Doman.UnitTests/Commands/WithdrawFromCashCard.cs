using System;
using Xunit;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.Commands;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.Commands
{
    public class WithdrawFromCashCardTests
    {
        [Fact]
        public void WithdrawFromCashCard_ShouldCreate()
        {
            var expectedId = Guid.NewGuid();
            var expectedPin = "4321";
            var expectedAmount = 56.78M;

            var command = new WithdrawFromCashCard(expectedId, expectedPin, expectedAmount);

            command.CashCardId.Should().Be(expectedId);
            command.Pin.Should().Be(expectedPin);
            command.AmountToWithdraw.Should().Be(expectedAmount);
        }
    }
}
