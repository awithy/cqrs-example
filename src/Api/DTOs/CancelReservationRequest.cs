namespace Api.DTOs
{
    public class CancelReservationRequest
    {
        public string MemberId { get; set; }
        public string ReservationId { get; set; }
    }
}