using System.Net;
using System.Threading.Tasks;
using Api.DTOs;
using Api.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Reservations
{
    [Route("api/v1/reservations")]
    public class ReservationsController : Controller
    {
        private readonly IReservationsFactory _reservationsFactory;
        private static readonly ILogger Log = new Logger(typeof(ReservationsController));

        public ReservationsController(IReservationsFactory reservationsFactory)
        {
            _reservationsFactory = reservationsFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Log.Debug("GET reservations");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateReservationRequest request)
        {
            Log.Debug("POST reservation");
            var reservation = await _reservationsFactory.Create(request);
            await reservation.Save();
            return Ok(new { id = reservation.Id });
        }

        [HttpPost("{id}/checkin")]
        public async Task<IActionResult> PostCheckin(string id, [FromBody] CheckIntoReservationRequest request)
        {
            Log.Debug($"POST reservation checkin for {id}");
            try
            {
                var reservation = await _reservationsFactory.Load(request.ReservationId);
                reservation.CheckIntoReservation(request.Method);
                await reservation.Save();
                return Ok();
            }
            catch (AlreadyCheckedInException)
            {
                return StatusCode((int) HttpStatusCode.Conflict, new {message = "Already checked in"});
            }
        }
    }
}