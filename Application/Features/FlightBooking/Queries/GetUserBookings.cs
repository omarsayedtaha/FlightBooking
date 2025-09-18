using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly AppHelperSerivices _appHelperSerivices;

        public GetUserBookings(IApplicationDbContext context, AppHelperSerivices appHelperSerivices)
        {
            _context = context;
            _appHelperSerivices = appHelperSerivices;
        }

        public async Task<PaginatedResponse<IEnumerable<BookingDto>>> GetAll()
        {
            var response = new PaginatedResponse<IEnumerable<BookingDto>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = null;

            var userId = await _appHelperSerivices.GetUserIdAsync();
            var booking = _context.Bookings.FirstOrDefault(b => b.UserId == userId);

            if (booking == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "You don't have any booking";
            }

            response.Data = booking.FlightBookings.Select(b => new BookingDto()
            {
                Id = b.Id,
                FlightId = b.FlightId,
                UserId = userId,
                BookingDate = booking.CreatedAt,
                NumberOfBookedSeats = b.Flight.NumberOfBookedSeats,
                Status = b.Status,
                TravelDate = b.DepartureDate,
                TotalCost = b.Price

            }).ToList();

            return response;
        }

    }
}
