using System;
using System.Collections;
using System.Threading.Tasks;
using Api.Common;
using Api.DTOs;
using Api.Events;
using Api.Modules.EventStore;

namespace Api.Modules.Reservations
{
    public interface IReservationsFactory
    {
        Task<Reservation> Create(CreateReservationRequest createReservationRequest);
        Task<Reservation> Load(string reservationId);
    }

    public class ReservationsFactory : IReservationsFactory
    {
        private readonly IEventStore _eventStore;

        public ReservationsFactory(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<Reservation> Create(CreateReservationRequest createReservationRequest)
        {
            var reservation = new Reservation(createReservationRequest);
            if(await _eventStore.StreamExists(reservation.Id))
                throw new ConflictException("Reservation already exists");
            reservation.OnSave = async x => { await _eventStore.AddEvents(x.Id, x.Events); };
            return reservation;
        }

        public async Task<Reservation> Load(string reservationId)
        {
            var events = await _eventStore.GetEvents(reservationId);
            var reservation = new Reservation(events);
            reservation.OnSave = async x => { await _eventStore.AddEvents(x.Id, x.Events); };
            return reservation;
        }
    }
}