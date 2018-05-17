using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Modules.EventStore
{
    public interface IEventStore
    {
        Task AddEvent<T>(string streamId, T @event) where T : EventStoreEvent;
        Task<IEnumerable<EventStoreEvent>> GetEvents(string streamId);
        Task<IEnumerable<EventStoreEvent>> GetEvents(string streamId, int fromSequence);
        Task AddEvents(string streamId, IEnumerable<EventStoreEvent> events);
        Task<bool> StreamExists(string streamId);
    }

    public class EventStore : IEventStore
    {
        private readonly ConcurrentDictionary<string, List<EventStoreEvent>> _events = new ConcurrentDictionary<string, List<EventStoreEvent>>();
        private readonly ConcurrentDictionary<string, int> _sequences = new ConcurrentDictionary<string, int>();
        private static readonly ConcurrentDictionary<string, object> LockObjects = new ConcurrentDictionary<string, object>();

        public Task AddEvent<T>(string streamId, T @event) where T : EventStoreEvent
        {
            _InitializeStream(streamId);

            lock (LockObjects[streamId])
                _AddEvent(streamId, @event);

            return Task.CompletedTask;
        }

        private void _AddEvent<T>(string id, T @event) where T : EventStoreEvent
        {
            var sequence = _sequences[id] + 1;
            _sequences[id] = sequence;
            @event.Sequence = sequence;
            @event.EventId = Guid.NewGuid().ToString("n");
            @event.Timestamp = DateTime.UtcNow;
            _events[@event.StreamId].Add(@event);
        }

        public Task<IEnumerable<EventStoreEvent>> GetEvents(string streamId)
        {
            return !_events.ContainsKey(streamId) 
                ? Task.FromResult<IEnumerable<EventStoreEvent>>(new EventStoreEvent[0]) 
                : Task.FromResult<IEnumerable<EventStoreEvent>>(_events[streamId]);
        }

        public Task<IEnumerable<EventStoreEvent>> GetEvents(string streamId, int fromSequence)
        {
            return !_events.ContainsKey(streamId) 
                ? Task.FromResult<IEnumerable<EventStoreEvent>>(new EventStoreEvent[0]) 
                : Task.FromResult(_events[streamId].Where(x => x.Sequence >= fromSequence));
        }

        public Task<bool> StreamExists(string streamId)
        {
            return Task.FromResult(_events.ContainsKey(streamId));
        }

        public Task AddEvents(string streamId, IEnumerable<EventStoreEvent> events)
        {
            _InitializeStream(streamId);

            lock (LockObjects[streamId])
                foreach (var @event in events)
                    _AddEvent(streamId, @event);

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
        public DateTime Timestamp { get; internal set; }
    }
}