using System.Linq;
using System.Threading.Tasks;
using Api.Modules.EventStore;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class EventStoreTests
    {
        [Fact]
        public async Task AddEventGetEvents()
        {
            var eventStore = new EventStore();
            var testEvent1 = new TestEvent { StreamId = "1", Data = "Initial" };
            var testEvent2 = new TestEvent { StreamId = "2", Data = "Initial" };
            var testEvent3 = new AnotherTestEvent { StreamId = "1", Data = "Updated" };
            await eventStore.AddEvent("1", testEvent1);
            await eventStore.AddEvent("2", testEvent2);
            await eventStore.AddEvent("1", testEvent3);
            var eventStream = await eventStore.GetEvents("1");
            eventStream.Count().Should().Be(2, "should retrieve all events in the stream");
        }

        [Fact]
        public async Task AddEvents()
        {
            var eventStore = new EventStore();
            var testEvent1 = new TestEvent { StreamId = "1", Data = "Initial" };
            var testEvent2 = new AnotherTestEvent { StreamId = "1", Data = "Updated" };
            await eventStore.AddEvents("1", new EventStoreEvent[] { testEvent1, testEvent2 });
            var eventStream = await eventStore.GetEvents("1");
            eventStream.Count().Should().Be(2, "should retrieve all events in the stream");
        }

        [Fact]
        public async Task GetEventsBySequence()
        {
            var eventStore = new EventStore();
            var testEvent1 = new TestEvent { StreamId = "1", Data = "Initial" };
            var testEvent2 = new AnotherTestEvent { StreamId = "1", Data = "Updated" };
            var testEvent3 = new AnotherTestEvent { StreamId = "1", Data = "Updated 2" };
            var testEvent4 = new AnotherTestEvent { StreamId = "1", Data = "Updated 3" };
            await eventStore.AddEvents("1", new EventStoreEvent[] { testEvent1, testEvent2, testEvent3, testEvent4 });
            var eventStream = (await eventStore.GetEvents("1", 2)).ToArray();
            eventStream.Count().Should().Be(2, "should retrieve only events >= sequence");
            eventStream.First().Sequence.Should().Be(2, "should return the event with sequence == 2");
            eventStream.Skip(1).First().Sequence.Should().Be(3, "should return the event with sequence == 3");
        }

        [Fact]
        public async Task StreamExists()
        {
            var eventStore = new EventStore();
            var exists = await eventStore.StreamExists("1");
            exists.Should().BeFalse("stream should not exist before an event is added");
            var testEvent1 = new TestEvent { StreamId = "1", Data = "Initial" };
            await eventStore.AddEvent("1", testEvent1);
            exists = await eventStore.StreamExists("1");
            exists.Should().BeTrue("stream should exist after an event is added");
        }

        public class TestEvent : EventStoreEvent
        {
            public string Data { get; set; }
        }

        public class AnotherTestEvent : EventStoreEvent
        {
            public string Data { get; set; }
        }
    }
}
