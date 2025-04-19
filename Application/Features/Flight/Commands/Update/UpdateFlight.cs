using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions.Dtos.Flight;
using CommonDefenitions;
using Microsoft.EntityFrameworkCore;
using Infrastructure;

namespace Application.Features.Flight.Commands.Update
{
    public class UpdateFlight
    {
        private readonly ApplicationDbContext _context;

        public UpdateFlight(ApplicationDbContext contex)
        {
            _context = contex;
        }
        public async Task<BaseResponse<Guid>> Update(Guid flightId , CreateFlightDto model)
        {
            var response = new BaseResponse<Guid>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = Guid.Empty;

            var flight =await _context.Flights.FirstOrDefaultAsync(f => f.Id == flightId);

            if (flight == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "flight not found";
                return response;
            } 
               
            flight.FlightNumber = model.FlightNumber;
            flight.Airline = model.Airline;
            flight.ArrivalLocation = model.ArrivalLocation;
            flight.ArrivalTime = model.ArrivalTime;
            flight.DepartureLocation = model.DepartureLocation;
            flight.DepartureTime = model.DepartureTime;
            flight.NumberOfSeats = model.NumberOfSeats;
            flight.Price = model.Price;
            flight.CreatedAt = DateTime.Now;
            

            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
            response.Message = "flight Updated successfully";
            response.Data = flight.Id;

            return response;

        }
    }
}
