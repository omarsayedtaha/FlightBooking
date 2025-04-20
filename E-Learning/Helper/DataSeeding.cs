using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
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

                var flights = new List<Flight>
                {
                    new Flight
                    {
                        Airline = "SkyWay Airlines",
                        FlightNumber ="SK100",
                        DepartureLocation = "New York, JFK",
                        ArrivalLocation = "London, LHR",
                        DepartureTime = DateTime.Now,
                        ArrivalTime = DateTime.Now.AddHours(2),
                        NumberOfSeats = 120,
                        NumberOfSeatsAvialable = 120,
                        Price = 349m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Flight
                    {
                        Airline = "SkyWay Airlines",
                        FlightNumber = "SK200",
                        DepartureLocation = "New York, JFK",
                        ArrivalLocation = "London, LHR",
                        DepartureTime = DateTime.Now,
                        ArrivalTime = DateTime.Now.AddHours(3),
                        NumberOfSeats = 95,
                        NumberOfSeatsAvialable=95,
                        Price = 429m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Flight
                    {
                        Airline = "SkyWay Airlines",
                        FlightNumber= "SK300",
                        DepartureLocation = "New York, JFK",
                        ArrivalLocation = "London, LHR",
                        DepartureTime = DateTime.Now.AddDays(1),
                        ArrivalTime = DateTime.Now.AddDays(1).AddHours(5),
                        NumberOfSeats = 150,
                        NumberOfSeatsAvialable = 150,
                        Price = 289m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }

                };
                context.Flights.AddRange(flights);

            }

            if (!context.Roles.Any())
            {
                var Roles = new List<IdentityRole<Guid>>
                        {
                            new IdentityRole<Guid> {Id=Guid.NewGuid(),Name ="Admin" , NormalizedName = "Admin" },
                            new IdentityRole<Guid> {Id=Guid.NewGuid() ,Name ="User" , NormalizedName = "User" }
                        };
                context.Roles.AddRange(Roles);  

            }

            await context.SaveChangesAsync();

        }
    }
}
