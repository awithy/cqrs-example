using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Modules.EventStore
{
    public interface IEventStore
    {
        Task AddEvent<T>(string id, T @event) where T : EventStoreEvent;
        Task<IEnumerable<EventStoreEvent>> GetEvents(string id);
        Task<IEnumerable<EventStoreEvent>> GetEvents(string id, int fromSequence);
        Task AddEvents(string id, IEnumerable<EventStoreEvent> events);
    }

    public class EventStore : IEventStore
    {
        private readonly ConcurrentDictionary<string, List<EventStoreEvent>> _events = new ConcurrentDictionary<string, List<EventStoreEvent>>();
        private readonly ConcurrentDictionary<string, int> _sequences = new ConcurrentDictionary<string, int>();
        private static readonly ConcurrentDictionary<string, object> LockObjects = new ConcurrentDictionary<string, object>();

        public Task AddEvent<T>(string id, T @event) where T : EventStoreEvent
        {
            _InitializeStream(id);

            lock (LockObjects[id])
                _AddEvent(id, @event);

            return Task.CompletedTask;
        }

        private void _AddEvent<T>(string id, T @event) where T : EventStoreEvent
        {
            var sequence = _sequences[id] + 1;
            _sequences[id] = sequence;
            @event.Sequence = sequence;
            @event.EventId = Guid.NewGuid().ToString("n");
            _events[@event.StreamId].Add(@event);
        }

        public Task<IEnumerable<EventStoreEvent>> GetEvents(string id)
        {
            return !_events.ContainsKey(id) 
                ? Task.FromResult<IEnumerable<EventStoreEvent>>(new EventStoreEvent[0]) 
                : Task.FromResult<IEnumerable<EventStoreEvent>>(_events[id]);
        }

        public Task<IEnumerable<EventStoreEvent>> GetEvents(string id, int fromSequence)
        {
            return !_events.ContainsKey(id) 
                ? Task.FromResult<IEnumerable<EventStoreEvent>>(new EventStoreEvent[0]) 
                : Task.FromResult(_events[id].Where(x => x.Sequence >= fromSequence));
        }

        public Task AddEvents(string id, IEnumerable<EventStoreEvent> events)
        {
            _InitializeStream(id);

            lock (LockObjects[id])
                foreach (var @event in events)
                    _AddEvent(id, @event);

            return Task.CompletedTask;
        }

        private void _InitializeStream(string id)
        {
            if (!LockObjects.ContainsKey(id))
                LockObjects.TryAdd(id, new List<EventStoreEvent>());

            if (!_events.ContainsKey(id))
                _events.TryAdd(id, new List<EventStoreEvent>());

            if (!_sequences.ContainsKey(id))
                _sequences.TryAdd(id, -1);
        }
    }

    public class EventStoreEvent
    {
        public string StreamId { get; set; }
        public string EventId { get; internal set; }
        public int Sequence { get; internal set; }
    }
}