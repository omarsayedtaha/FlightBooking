using Application.Common;
using Application.Features.Booking.Commands;
using Application.Features.Booking.Commands.Delete;
using Application.Features.Booking.Queries;
using Application.Features.FlightBooking.Commands.Update;
using Application.Interfaces;
using Booking.Helper;
using Domain.Entities;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlightBookingController : ControllerBase
    {
        private readonly IApplicationDbContext context;
        private readonly AppHelperSerivices appHelper;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;
        private readonly ConfirmBooking updateBooking;

        public FlightBookingController(IApplicationDbContext context,
            AppHelperSerivices appHelper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor
            , UserManager<User> userManager)
        {
            this.context = context;
            this.appHelper = appHelper;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.updateBooking = updateBooking;
        }

        [HttpPost("book-a-seat")]
        public async Task<IActionResult> BookASeat([FromBody] SeatBookingRequest request)
        {
            var bookingservice = new BookSeat(context, appHelper, userManager);
            var result = await bookingservice.Book(request);

            return Ok(result);
        }

        [HttpGet("confirm-booking")]
        public async Task<IActionResult> ConfirmBooking()
        {
            var bookingservice = new ConfirmBooking(context, appHelper);
            var result = await bookingservice.Confirm();

            return Ok(result);
        }

        [HttpPost("Update-booking-status")]
        public async Task<IActionResult> UpdateBooking(string sessionId)
        {
            var bookingservice = new UpdateBookingStatus(context);
            var result = await bookingservice.Update(sessionId);

            return Ok(result);
        }

        [HttpPost("get-user-booking-details")]
        public async Task<IActionResult> GetUserBookings()
        {
            var bookingservice = new GetUserBookings(context, appHelper);
            var result = await bookingservice.GetAll();

            return Ok(result);
        }

        [HttpPost("cancel-booking")]
        public async Task<IActionResult> CancelBooking([FromBody] int bookingId)
        {
            var bookingservice = new DeleteBooking(context);
            var result = await bookingservice.CancelBooking(bookingId);

            return Ok(result);
        }
    }
}
