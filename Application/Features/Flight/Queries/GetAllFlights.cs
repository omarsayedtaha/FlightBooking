using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using CommonDefenitions.Dtos.Flight;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Flight.Queries
{
    public class GetAllFlights
    {
        private readonly ApplicationDbContext context;
        private readonly BaseRequest request;

        public GetAllFlights(ApplicationDbContext context , BaseRequest request)
        {
            this.context = context;
            this.request = request;
        }

        public async Task<BaseResponse<IEnumerable<FlightDto>>> GetAll()
        {
            var response = new BaseResponse<IEnumerable<FlightDto>>();  
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            
            response.Data = context.Flights.Select(f=> new FlightDto
            {
                Id = f.Id,
                Airline = f.Airline,
                DepartureLocation = f.DepartureLocation,
                DepartureDate = f.DepartureTime.ToString("d"),
                DepartureTime = f.DepartureTime.ToString("t"),
                ArrivalLocation = f.ArrivalLocation,
                ArrivalDate = f.ArrivalTime.ToString("d"),
                ArrivalTime = f.ArrivalTime.ToString("t"),
                Duration = f.CalculateDuration(f.DepartureTime, f.ArrivalTime),
                FlightNumber = f.FlightNumber,  
                Price = f.Price 
            }).ToList();
            
            return response;    
        }

    }
}
