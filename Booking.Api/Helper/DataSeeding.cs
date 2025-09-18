using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Domian.Enums;
using Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace CommonDefenitions.Helper
{
    public static class DataSeeding
    {

        public static async void SeedData(ApplicationDbContext context)
        {
            if (!context.Flights.Any())
            {

                var flights = new List<Flight>()
                {
                     new Flight
                {
                    Id = 1,
                    FlightNumber = "MS101",
                    Airline = "EgyptAir",
                    DepartureLocation = "CAI",
                    ArrivalLocation = "DXB",
                    DepartureTime = new DateTime(2025, 9, 20, 10, 0, 0),
                    ArrivalTime = new DateTime(2025, 9, 20, 14, 0, 0),
                    HasArrived = false,
                    NumberOfSeats = 180,
                    NumberOfSeatsAvialable = 160,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flight
                {
                    Id = 2,
                    FlightNumber = "EK202",
                    Airline = "Emirates",
                    DepartureLocation = "DXB",
                    ArrivalLocation = "LHR",
                    DepartureTime = new DateTime(2025, 9, 21, 16, 0, 0),
                    ArrivalTime = new DateTime(2025, 9, 21, 21, 0, 0),
                    HasArrived = false,
                    NumberOfSeats = 200,
                    NumberOfSeatsAvialable = 180,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },


                };

                // if (!context.Seats.Any())
                // {
                //     var seats = new List<Seat>
                //     {
                //         new Seat {Id=1, SeatNumber = "1A", FlightId = 1 },
                //         new Seat {Id=2, SeatNumber = "1B", FlightId = 1 },
                //         new Seat {Id=3, SeatNumber = "2A", FlightId = 1 },

                //         new Seat {Id=1, SeatNumber = "1A", FlightId = 2 },
                //         new Seat {Id=2, SeatNumber = "1B", FlightId = 2 },
                //         new Seat {Id=3, SeatNumber = "2A", FlightId = 2 }
                //     };
                // }


                context.Flights.AddRange(flights);
                // context.Seats.AddRange(seats);

            }

            // if (!context.Roles.Any())
            // {
            //     var Role = new IdentityRole
            //     {
            //         Name = "SuperAdmin",
            //         NormalizedName = "SuperAdmin"
            //     };
            //     context.Roles.Add(Role);

            // }

            await context.SaveChangesAsync();

        }
    }
}
