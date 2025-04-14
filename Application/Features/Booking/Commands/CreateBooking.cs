using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using CommonDefenitions.Dtos.Bookings;
using Infrastructure;

namespace Application.Features.Booking.Commands
{
    public class CreateBooking
    {
        private readonly ApplicationDbContext _context;

        public CreateBooking(ApplicationDbContext context)
        {
            _context = context;
        }

        //public BaseResponse<BookingDto> Create(Guid? flightId)
        //{
        //    var response = new BaseResponse<BookingDto>();
        //    response.StatusCode = HttpStatusCode.OK;
        //    response.Message = null;
        //    response.Data = null;

        //    if (!flightId.HasValue)
        //    {
        //        response.StatusCode = HttpStatusCode.BadRequest;
        //        response.Message = "FlightId is required";
        //        return response;
        //    }
        //    var flight = _context.Flights.FirstOrDefault(f => f.Id == flightId);
        //    if (flight == null)
        //    {
        //        response.StatusCode = HttpStatusCode.NotFound;
        //        response.Message = "Flight not found";
        //        return response;
        //    }

        //}
    }
}
