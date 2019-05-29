
using System;
using Xunit;
using Moq;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using CreditSuisse.VirtualCashCard.Doman.CommandHandlers;
using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.Commands;
using FluentAssertions;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.CommandHandlers
{
    public class CashCardCommandHandlerTests
    {
        [Fact]
        public void CashCardCommandHandler_ShouldCreate()
        {
            var eventStore = new Mock<IEventStore>();

            new CashCardCommandHandler(eventStore.Object);
        }

        [Fact]
        public void CashCardCommandHandler_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new CashCardCommandHandler(null));
        }

        [Fact]
        public void CashCardCommandHandler_ShouldHandle_CreateCashCard()
        {
            var id = Guid.NewGuid();

            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(x => x.Save<CashCard>(It.IsAny<CashCard>()));

            var handler = new CashCardCommandHandler(eventStore.Object);

            handler.Handle(new CreateCashCard(id, "1234"));

            eventStore.Verify(x => x.Save<CashCard>(It.Is<CashCard>(c => c.Id == id && c.Balance == 0M)));
        }

        [Fact]
        public void CashCardCommandHandler_ShouldHandle_TopUpCashCard()
        {
            var id = Guid.NewGuid();

            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(x => x.GetById<CashCard>(id)).Returns(new CashCard(id, "1234"));
            eventStore.Setup(x => x.Save<CashCard>(It.IsAny<CashCard>()));

            var handler = new CashCardCommandHandler(eventStore.Object);

            handler.Handle(new TopUpCashCard(id, 400M));

            eventStore.Verify(x => x.Save<CashCard>(It.Is<CashCard>(c => c.Id == id && c.Balance == 400M)));
        }

        [Fact]
        public void CashCardCommandHandler_ShouldHandle_WithdrawFromCashCard()
        {
            var id = Guid.NewGuid();
            var cashCard = new CashCard(id, "1234");
            cashCard.TopUp(100);

            var eventStore = new Mock<IEventStore>();
            eventStore.Setup(x => x.GetById<CashCard>(id)).Returns(cashCard);
            eventStore.Setup(x => x.Save<CashCard>(It.IsAny<CashCard>()));

            var handler = new CashCardCommandHandler(eventStore.Object);

            handler.Handle(new WithdrawFromCashCard(id, "1234", 50M));

            eventStore.Verify(x => x.Save<CashCard>(It.Is<CashCard>(c => c.Id == id && c.Balance == 50M)));
        }
    }
}
