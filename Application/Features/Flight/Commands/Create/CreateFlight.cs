using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.Flight;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Flight.Commands.Create
{
    public class CreateFlight
    {
        private readonly IApplicationDbContext _context;

        public CreateFlight(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<BaseResponse<Guid>> Create(CreateFlightDto model)
        {
            var response = new BaseResponse<Guid>();
            response.Message = string.Empty;
            response.Data = Guid.Empty;

            var Validator = new CreateFlightValidator();
            var result = Validator.Validate(model);

            if (!result.IsValid)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = string.Join(",", result.Errors.Select(x => x.ErrorMessage));
                return response;
            }

            var IsFlightExist = _context.Flights
                .Any(f => f.FlightNumber == model.FlightNumber &&
                 f.Airline == model.Airline &&
                 f.ArrivalLocation == model.ArrivalLocation &&
                 f.DepartureLocation == model.DepartureLocation &&
                 f.DepartureTime == model.DepartureTime &&
                 f.NumberOfSeats == model.NumberOfSeats &&
                 f.Price == model.Price &&
                 f.CreatedAt == DateTime.Now);

            if (IsFlightExist)
            {
                response.Message = "This flight is already created";
                return response;
            }

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


            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            response.StatusCode = HttpStatusCode.OK;
            response.Message = "flight Craeted successfully";
            response.Data = flight.Id;

            return response;

        }
    }
}
