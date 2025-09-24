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
                    FlightNumber = "MS101",
                    Airline = "EgyptAir",
                    DepartureLocation = "CAI",
                    ArrivalLocation = "DXB",
                    DepartureTime = new DateTime(2025, 9, 27, 10, 0, 0),
                    ArrivalTime = new DateTime(2025, 9, 27, 14, 0, 0),
                    HasArrived = false,
                    NumberOfSeats = 180,
                    NumberOfSeatsAvialable = 160,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flight
                {
                    FlightNumber = "EK202",
                    Airline = "Emirates",
                    DepartureLocation = "DXB",
                    ArrivalLocation = "LHR",
                    DepartureTime = new DateTime(2025, 9, 29, 16, 0, 0),
                    ArrivalTime = new DateTime(2025, 9, 29, 21, 0, 0),
                    HasArrived = false,
                    NumberOfSeats = 200,
                    NumberOfSeatsAvialable = 180,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },


                };
                context.Flights.AddRange(flights);
                await context.SaveChangesAsync();

            }

            if (!context.FlightSeatClass.Any())
            {
                var flightseats = new List<FlightSeatClass>
                {
                        new FlightSeatClass
                        {
                            Class = SeatClass.Economy,
                            Price = 200,
                            FlightId = 1
                        },
                        new FlightSeatClass
                        {
                            Class = SeatClass.Business,
                            Price = 500,
                            FlightId = 1
                        },
                        new FlightSeatClass
                        {
                            Class = SeatClass.First,
                            Price = 900,
                            FlightId = 1
                        },

                        new FlightSeatClass
                        {
                            Class = SeatClass.Economy,
                            Price = 250,
                            FlightId = 2
                        },
                        new FlightSeatClass
                        {
                            Class = SeatClass.Business,
                            Price = 550,
                            FlightId = 2
                        },
                        new FlightSeatClass
                        {
                            Class = SeatClass.First,
                            Price = 950,
                            FlightId = 2
                        }
                };

                context.FlightSeatClass.AddRange(flightseats);
                await context.SaveChangesAsync();
            }


            if (!context.Seats.Any())
            {
                var seats = new List<Seat>
                {
                    new Seat { SeatNumber = "1A", FlightId = 1,FlightSeatClassId=1,IsBooked=false },
                    new Seat {SeatNumber = "1B", FlightId = 1,FlightSeatClassId=1,IsBooked=false },
                    new Seat {SeatNumber = "2A", FlightId = 1 , FlightSeatClassId=2 , IsBooked=false },

                    new Seat {SeatNumber = "1A", FlightId = 2 , FlightSeatClassId=3, IsBooked=false },
                    new Seat {SeatNumber = "1B", FlightId = 2 , FlightSeatClassId=1,IsBooked=false},
                    new Seat {SeatNumber = "2A", FlightId = 2 , FlightSeatClassId = 2 , IsBooked=false}
                };
                context.Seats.AddRange(seats);
            }

            await context.SaveChangesAsync();


        }
    }
}
