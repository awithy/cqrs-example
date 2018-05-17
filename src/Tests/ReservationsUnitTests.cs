using System.Linq;
using Api.DTOs;
using Api.Events;
using Api.Modules.Reservations;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class ReservationsUnitTests
    {
        [Fact]
        public void Create()
        {
            var request = new CreateReservationRequest
            {
                MemberId = "memberId1",
                SessionId = "sessionId1",
                SessionLocation = "sessionLocation",
            };
            var reservation = new Reservation(request);
            reservation.Id.Should().Be("reservation-sessionId1-memberId1");
            reservation.Events.Count.Should().Be(1, "should create an event on created");
            var firstEvent = reservation.Events.First() as ReservationCreatedEvent;
            firstEvent.Should().NotBeNull("should create reservation created event");
            // ReSharper disable once PossibleNullReferenceException
            firstEvent.ReservationId.Should().Be(reservation.Id);
            firstEvent.MemberId.Should().Be(request.MemberId, "should have correct memberId");
            firstEvent.SessionId.Should().Be(request.SessionId, "should have correct sessionId");
            firstEvent.SessionLocation.Should().Be(request.SessionLocation, "should have correct session location");
        }
    }
}