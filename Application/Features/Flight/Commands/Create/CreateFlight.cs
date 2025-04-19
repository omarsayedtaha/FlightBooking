using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using CommonDefenitions.Dtos.Flight;
using Infrastructure;

namespace Application.Features.Flight.Commands.Create
{
    public class CreateFlight
    {
        private readonly ApplicationDbContext _context;

        public CreateFlight(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<Guid>> Create(CreateFlightDto model)
        {
            var response = new BaseResponse<Guid>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = Guid.Empty;

            var IsFlightExist = _context.Flights
                .Any(f => f.FlightNumber == model.FlightNumber &&
                 f.Airline == model.Airline &&
                 f.ArrivalLocation == model.ArrivalLocation &&
                 f.DepartureLocation == model.DepartureLocation &&
                 f.DepartureTime == model.DepartureTime &&
                 f.NumberOfSeats == model.NumberOfSeats &&
                 f.Price == model.Price &&
                 f.CreatedAt == DateTime.Now);

            var flight = new Domain.Flight
            {
                FlightNumber = model.FlightNumber,
                Airline = model.Airline,
                ArrivalLocation = model.ArrivalLocation,
                ArrivalTime = model.ArrivalTime,
                DepartureLocation = model.DepartureLocation,
                DepartureTime = model.DepartureTime,
                NumberOfSeats = model.NumberOfSeats,
                Price = model.Price,
                CreatedAt = DateTime.Now,
            };

            if (IsFlightExist)
            {
                response.Message = "This flight is already created";
                return response;
            }
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            response.Message = "flight Craeted successfully";
            response.Data = flight.Id;

            return response;

        }
    }
}
