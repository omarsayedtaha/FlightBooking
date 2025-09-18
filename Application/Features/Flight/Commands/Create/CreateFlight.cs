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
using Domain.Entities;
using Domian.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Flight.Commands.Create
{
    public class CreateFlight
    {
        private readonly IApplicationDbContext _context;

        public static readonly string[] SeatNumbers =
        {
            "1A","1B","1C","1D",
            "2A","2B","2C","2D",
            "3A","3B","3C","3D"
        };
        public CreateFlight(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<int>> Create(CreateFlightDto model)
        {
            var response = new BaseResponse<int>();
            response.Message = string.Empty;
            response.Data = 0;

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
                 f.CreatedAt == DateTime.Now);

            if (IsFlightExist)
            {
                response.Message = "This flight is already created";
                return response;
            }
            var flight = new Domain.Entities.Flight
            {
                FlightNumber = model.FlightNumber,
                Airline = model.Airline,
                ArrivalLocation = model.ArrivalLocation,
                ArrivalTime = model.ArrivalTime,
                DepartureLocation = model.DepartureLocation,
                DepartureTime = model.DepartureTime,
                NumberOfSeats = model.NumberOfSeats,
                CreatedAt = DateTime.Now,
            };

            _context.Flights.Add(flight);


            var SeatClass = new FlightSeatClass()
            {
                FlightId = flight.Id,
                Class = (SeatClass)Enum.Parse(typeof(SeatClass), model.Class),
                Price = model.Price
            };

            _context.FlightSeatClass.Add(SeatClass);


            var Seats = new List<Seat>();
            for (int i = 0; i < SeatNumbers.Count(); i++)
            {
                Seats.Add(new Seat()
                {
                    FlightId = flight.Id,
                    SeatNumber = SeatNumbers[i],
                    IsBooked = false
                });
            }


            _context.Seats.AddRange(Seats);
            await _context.SaveChangesAsync();

            response.StatusCode = HttpStatusCode.OK;
            response.Message = "flight Craeted successfully";
            response.Data = flight.Id;

            return response;

        }


    }
}
