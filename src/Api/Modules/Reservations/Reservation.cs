using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Events;
using Api.Modules.EventStore;

namespace Api.Modules.Reservations
{
    public class Reservation
    {
        public readonly List<EventStoreEvent> Events = new List<EventStoreEvent>();
        public string Id { get; set; }
        public string CheckInMethod { get; set; }
        public bool CheckedIn { get; set; }
        public string SessionId { get; set; }
        public string MemberId { get; set; }

        public Reservation(CreateReservationRequest createReservationRequest)
        {
            var streamId = $"reservation-{createReservationRequest.SessionId}-{createReservationRequest.MemberId}";
            var reservationCreatedEvent = new ReservationCreatedEvent
            {
                StreamId = streamId,
                ReservationId =  streamId,
                SessionId = createReservationRequest.SessionId,
                MemberId = createReservationRequest.MemberId,
                SessionLocation = createReservationRequest.SessionLocation,
            };
            _AddEvent(reservationCreatedEvent);
        }

        public Reservation(IEnumerable<EventStoreEvent> events)
        {
            foreach(var @event in events)
                _SourceEvent(@event);
        }

        public void CheckIntoReservation(string method)
        {
            if (CheckedIn)
                throw new AlreadyCheckedInException();
            _AddEvent(new ReservationCheckedInEvent {Method = method});
        }

        private void _AddEvent(EventStoreEvent @event)
        {
            _SourceEvent(@event);
            Events.Add(@event);
        }

        private void _SourceEvent(EventStoreEvent @event)
        {
            switch (@event)
            {
                case ReservationCreatedEvent createdEvent:
                    _Created(createdEvent);
                    break;
                case ReservationCheckedInEvent checkedInEvent:
                    _CheckedIn(checkedInEvent);
                    break;
            }
        }

        private void _Created(ReservationCreatedEvent @event)
        {
            Id = @event.ReservationId;
            SessionId = @event.SessionId;
            MemberId = @event.MemberId;
        }

        private void _CheckedIn(ReservationCheckedInEvent @event)
        {
            CheckedIn = true;
            CheckInMethod = @event.Method;
        }

        public Func<Reservation,Task> OnSave;

        public async Task Save()
        {
            var onSave = OnSave;
            if(onSave != null)
                await onSave(this);
        }
    }
}