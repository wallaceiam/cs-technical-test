using CreditSuisse.VirtualCashCard.Doman.AggregateRoots;
using CreditSuisse.VirtualCashCard.Doman.Interfaces;
using FluentAssertions;
using System;
using Xunit;

namespace CreditSuisse.VirtualCashCard.Doman.UnitTests.AggregateRoots
{
    public class AggregateRootTests
    {
        [Fact]
        public void AggregateRoot_ShouldCreate()
        {
            var aggregate = new TestAggregateRoot();

            aggregate.Version.Should().Be(0);
            aggregate.GetUncommittedChanges().Should().BeEmpty();
        }

        [Fact]
        public void AggregateRoot_ShouldLoadEventsFromHistory()
        {
            var aggregate = new TestAggregateRoot();

            aggregate.LoadEventsFromHistory(new[] { new TestEvent(1) });
            aggregate.GetUncommittedChanges().Should().BeEmpty();
            aggregate.Version.Should().Be(1);
        }

        [Fact]
        public void AggregateRoot_ShouldMarkChangesAsCommitted()
        {
            var aggregate = new TestAggregateRoot();

            aggregate.CreateEvent();
            aggregate.GetUncommittedChanges().Should().NotBeNullOrEmpty();

            aggregate.MarkChangesAsCommitted();
            aggregate.GetUncommittedChanges().Should().BeEmpty();

        }

        internal class TestAggregateRoot : AggregateRoot
        {
            public override Guid Id { get; }

            public void CreateEvent()
            {
                this.ApplyChange(new TestEvent(this.Version));
            }

            private void Apply(TestEvent @event)
            {
                this.Version = @event.Version;
            }
        }

        internal class TestEvent : IEvent
        {
            public TestEvent(int version = 0)
            {
                this.SetVersion(version);
            }
            public int Version { get; private set; }

            public void SetVersion(int version)
            {
                this.Version = version;
            }
        }
    }
}
