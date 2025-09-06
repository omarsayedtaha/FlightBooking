using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;

namespace Application.Features.Booking.Commands.Delete
{
    public class DeleteBooking
    {
        private readonly IApplicationDbContext _context;

        public DeleteBooking(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<Guid>> CancelBooking(Guid bookingId)
        {
            var response = new BaseResponse<Guid>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = Guid.Empty;

            if (bookingId == Guid.Empty)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "flightId is required";
                return response;
            }

            var booking = _context.Bookings.FirstOrDefault(f => f.Id == bookingId);

            if (booking == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "flight not found";
                return response;
            }

            booking.IsCanceled = true;
            booking.CancellationDate = DateTime.Now;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            response.Message = "Booking cancelled";
            response.Data = booking.Id;
            return response;
        }
    }
}
