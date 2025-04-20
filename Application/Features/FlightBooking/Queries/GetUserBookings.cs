using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions.Dtos.Flight;
using CommonDefenitions;
using Infrastructure;
using CommonDefenitions.Dtos.Bookings;

namespace Application.Features.Booking.Queries
{
    public class GetUserBookings
    {
        private readonly ApplicationDbContext _context;

        public GetUserBookings(ApplicationDbContext context)
        {
           _context = context;
        }

        public async Task<BaseResponse<IEnumerable<BookingDto>>> GetAll(Guid UserId)
        {
            var response = new BaseResponse<IEnumerable<BookingDto>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = null;

            var booking = _context.Bookings.Where(b=>b.UserId==UserId && !b.IsCanceled).ToList();

            if (booking==null)
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
