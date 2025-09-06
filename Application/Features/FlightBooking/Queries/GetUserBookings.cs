using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.Bookings;
using Application.Interfaces;


namespace Application.Features.Booking.Queries
{
    public class GetUserBookings
    {
        private readonly IApplicationDbContext _context;

        public GetUserBookings(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<IEnumerable<BookingDto>>> GetAll(Guid UserId)
        {
            var response = new PaginatedResponse<IEnumerable<BookingDto>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = null;

            var booking = _context.Bookings.Where(b => b.UserId == UserId && !b.IsCanceled).ToList();

            if (booking == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "You don't have any booking";
            }

            response.Data = booking.Select(b => new BookingDto()
            {
                Id = b.Id,
                FlightId = b.FlightId,
                UserId = b.UserId,
                BookingDate = b.BookingDate,
                NumberOfBookedSeats = b.NumberOfBookedSeats,
                Status = b.Status,
                TravelDate = b.TravelDate,
                TotalCost = b.TotalCost,

            }).ToList();

            return response;
        }

    }
}
