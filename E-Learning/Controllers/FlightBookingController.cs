using Application.Features.Booking.Commands;
using Booking.Helper;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightBookingController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly AppHelperSerivices appHelper;

        public FlightBookingController(ApplicationDbContext context,AppHelperSerivices appHelper)
        {
            this.context = context;
            this.appHelper = appHelper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromQuery] Guid flightId)
        {
            var bookkingservice = new CreateBooking(context, appHelper);
            var result =await bookkingservice.Create(flightId);

            return Ok(result);  
        }
    }
}
