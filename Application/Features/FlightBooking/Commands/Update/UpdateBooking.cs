using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Booking.Commands.Update
{
    public class UpdateBooking
    {
        private readonly ApplicationDbContext _context;

        public UpdateBooking(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BaseResponse<Guid>> ConfirmBooking(Guid bookingId)
        {
            var response = new BaseResponse<Guid>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = Guid.Empty;

            if (bookingId == Guid.Empty)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "BookingId is required";
                return response;
            }
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
            {
                response.Message = "Booking not found";
                return response;
            }
            booking.Flight.NumberOfSeatsAvialable--;
            _context.Flights.Update(booking.Flight);

            booking.Status = BookingStatus.Succeded.ToString();
            _context.Bookings.Update(booking);

            await _context.SaveChangesAsync();
            response.Message = "booking Confirmed";

            return response;

        }
    }
}
