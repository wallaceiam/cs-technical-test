using System;
using Xunit;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.Commands;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.Commands
{
    public class CreateCashCardTests
    {
        [Fact]
        public void CreateCashCard_ShouldCreate()
        {
            var expectedId = Guid.NewGuid();
            var expectedPin = "5678";

            var command = new CreateCashCard(expectedId, expectedPin);

            command.CashCardId.Should().Be(expectedId);
            command.Pin.Should().Be(expectedPin);
        }
    }
}
