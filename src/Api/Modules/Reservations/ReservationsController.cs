using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Reservations
{
    [Route("api/v1/reservations")]
    public class ReservationsController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}