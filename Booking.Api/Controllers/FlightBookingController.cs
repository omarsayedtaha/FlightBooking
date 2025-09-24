using Application.Common;
using Application.Common.services;
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
        private readonly BookSeat bookSeat;
        private readonly ConfirmBooking confirmBooking;
        private readonly UpdateBookingStatus updateBookingStatus;
        private readonly GetUserBookings getUserBookings;
        private readonly DeleteBooking deleteBooking;

        public FlightBookingController(BookSeat bookSeat,
        ConfirmBooking confirmBooking,
        UpdateBookingStatus updateBookingStatus,
        GetUserBookings getUserBookings,
        DeleteBooking deleteBooking)
        {
            this.bookSeat = bookSeat;
            this.confirmBooking = confirmBooking;
            this.updateBookingStatus = updateBookingStatus;
            this.getUserBookings = getUserBookings;
            this.deleteBooking = deleteBooking;
        }

        [HttpPost("book-a-seat")]
        public async Task<IActionResult> BookASeat([FromBody] SeatBookingRequest request)
        {
            var result = await bookSeat.Book(request);

            return Ok(result);
        }

        [HttpGet("confirm-booking")]
        public async Task<IActionResult> ConfirmBooking()
        {
            var result = await confirmBooking.Confirm();

            return Ok(result);
        }

        [HttpPost("Update-booking-status")]
        public async Task<IActionResult> UpdateBooking(string sessionId)
        {
            var result = await updateBookingStatus.Update(sessionId);

            return Ok(result);
        }

        [HttpPost("get-user-booking-details")]
        public async Task<IActionResult> GetUserBookings()
        {
            var result = await getUserBookings.GetAll();

            return Ok(result);
        }

        [HttpPost("cancel-booking")]
        public async Task<IActionResult> CancelBooking([FromBody] int bookingId)
        {
            var result = await deleteBooking.CancelBooking(bookingId);

            return Ok(result);
        }
    }
}
