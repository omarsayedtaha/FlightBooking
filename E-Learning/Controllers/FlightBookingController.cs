using Application.Features.Booking.Commands;
using Application.Features.Booking.Commands.Create;
using Application.Features.Booking.Commands.Delete;
using Application.Features.Booking.Commands.Update;
using Application.Features.Booking.Queries;
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
        private readonly UpdateBooking updateBooking;

        public FlightBookingController(ApplicationDbContext context,
            AppHelperSerivices appHelper)
        {
            this.context = context;
            this.appHelper = appHelper;
            this.updateBooking = updateBooking;
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
            var bookingservice = new UpdateBooking(context);
            var result =await bookingservice.ConfirmBooking(bookingId);

            return Ok(result);
        }

        [HttpPost("get-user-booking-details")]
        public async Task<IActionResult> GetUserBookings([FromQuery]Guid UserId)
        {
            var bookingservice = new GetUserBookings(context);
            var result = await bookingservice.GetAll(UserId);

            return Ok(result);
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
