using Api.Modules.EventStore;

namespace Api.Events
{
    public class ReservationCreatedEvent : EventStoreEvent
    {
        public string ReservationId { get; set; }
        public string SessionId { get; set; }
        public string MemberId { get; set; }
        public string SessionLocation { get; set; }
    }
}