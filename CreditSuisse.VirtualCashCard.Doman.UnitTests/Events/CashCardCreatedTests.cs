using System;
using Xunit;
using FluentAssertions;
using CreditSuisse.VirtualCashCard.Doman.Events;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.Events
{
    public class CashCardCreatedTests
    {
        [Fact]
        public void CashCardCreated_ShouldCreate()
        {
            var expectedId = Guid.NewGuid();
            var expectedPin = "5678";
            var expectedVersion = 3;

            var @event = new CashCardCreated(expectedId, expectedPin, expectedVersion);

            @event.CashCardId.Should().Be(expectedId);
            @event.Pin.Should().Be(expectedPin);
            @event.Version.Should().Be(expectedVersion);
        }
    }
}
