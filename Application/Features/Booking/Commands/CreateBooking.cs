using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Booking.Helper;
using CommonDefenitions;
using CommonDefenitions.Dtos.Bookings;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Booking.Commands
{
    public class CreateBooking
    {
        private readonly ApplicationDbContext _context;
        private readonly AppHelperSerivices _appHelper;

        public CreateBooking(ApplicationDbContext context , AppHelperSerivices appHelper)
        {
            _context = context;
            _appHelper = appHelper;
        }

        public async Task<BaseResponse<Guid>> Create(Guid flightId)
        {
            var response = new BaseResponse<Guid>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = Guid.Empty;

            if (flightId==Guid.Empty)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "FlightId is required";
                return response;
            }
            var flight = new Domain.Flight();
            if (_context.Bookings.Any())
                 flight =await _context.Flights.Where(f => f.Id == flightId).Include(b => b.FlightBookings).FirstOrDefaultAsync();
            else 
               flight =await _context.Flights.Where(f => f.Id == flightId).FirstOrDefaultAsync();

            if (flight == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Flight not found";
                return response;
            }

            var booking = new Domain.FlightBookings
            {
                FlightId = flight.Id,
                BookingDate = DateTime.Now,
                TravelDate = flight.DepartureTime,
                UserId = await _appHelper.GetUserIdAsync(),
                TotalCost = flight.Price,
                CreatedAt = DateTime.Now,
                Status = BookingStatus.Pending.ToString(),
                NumberOfBookedSeats = flight.FlightBookings != null ? flight.FlightBookings.Where(b => b.FlightId == flightId).Count()
                : 1,
            };

            _context.Bookings.Add(booking);

           await _context.SaveChangesAsync(); 

            response.Data = booking.Id;
            response.Message = $"Booking {booking.Status}";
            return response;

        }
    }
}
