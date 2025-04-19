using Application.Features.Booking.Commands;
using Application.Features.Booking.Commands.Create;
using Application.Features.Booking.Commands.Delete;
using Booking.Helper;
using CommonDefenitions;
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

        [HttpPost("create-booking")]
        public async Task<IActionResult> CreateBooking([FromQuery] Guid flightId)
        {
            var bookingservice = new CreateBooking(context, appHelper);
            var result =await bookingservice.Create(flightId);

            return Ok(result);  
        }

        [HttpPost("confirm-booking")]
        public async Task<IActionResult> ConfirmBooking([FromBody] Guid bookingId)
        {
            var booking = context.Bookings.FirstOrDefault(b=>b.Id==bookingId);
            booking.Status = BookingStatus.Succeded.ToString();
            return Ok("Booking confirmed");
        }

        [HttpPost("cancel-booking")]
        public async Task<IActionResult> CancelBooking([FromBody] Guid bookingId)
        {
            var bookingservice = new DeleteBooking(context);
            var result = await bookingservice.CancelBooking(bookingId);

            return Ok(result);
        }
    }
}
