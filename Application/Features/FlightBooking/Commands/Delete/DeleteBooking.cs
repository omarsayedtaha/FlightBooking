using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Domian.Enums;

namespace Application.Features.Booking.Commands.Delete
{
    public class DeleteBooking
    {
        private readonly IApplicationDbContext _context;

        public DeleteBooking(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<int>> CancelBooking(int bookingId)
        {
            var response = new BaseResponse<int>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = 0;

            if (bookingId == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "flightId is required";
                return response;
            }

            var booking = _context.Bookings.FirstOrDefault(f => f.Id == bookingId && f.Status == BookingStatus.Pending.ToString());

            if (booking == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "flight not found";
                return response;
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            response.Message = "Booking cancelled";
            return response;
        }
    }
}
