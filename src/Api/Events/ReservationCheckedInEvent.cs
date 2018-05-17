using Api.Modules.EventStore;

namespace Api.Events
{
    public class ReservationCheckedInEvent : EventStoreEvent
    {
        public string Method { get; set; }
    }
}