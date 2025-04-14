using Application.Features.Flight.Queries;
using CommonDefenitions;
using CommonDefenitions.Dtos.Flight;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BaseRequest _request;

        public FlightController(ApplicationDbContext context , BaseRequest request)
        {
            _context = context;
            _request = request;
        }
        [HttpGet("getflightswithfilter")]
        public async Task<IActionResult> Get([FromQuery]FlightFilterDto flightFilter)
        {
            var Service = new GetFlightsWithFilter(_context);
            var flights = Service.Get(_request, flightFilter);
            return Ok(flights); 
        }
    }
}