using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using CommonDefenitions.Dtos.Flight;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Flight.Queries
{
    public class GetFlightsWithFilter
    {
        private readonly ApplicationDbContext _context;

        public GetFlightsWithFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BaseResponse<IEnumerable<FlightDto>>> Get(BaseRequest request, FlightFilterDto filter)
        {
            BaseResponse<IEnumerable<FlightDto>> response = new BaseResponse<IEnumerable<FlightDto>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = null;
            response.Data = null;

            var query = _context.Flights.AsQueryable();
            if (filter.Id.HasValue)
            {

                query = _context.Flights.Where(f => f.Id == filter.Id.Value );

            }
            if (filter.Date.HasValue)
            {

                query = _context.Flights.Where(f => f.DepartureTime.Date == filter.Date.Value.ToDateTime(TimeOnly.MinValue));

            }

            if (!string.IsNullOrEmpty(request.Orderby))
            {
                query = request.IsAscending ?
                    query.OrderBy(f => request.Orderby)
                    : query.OrderByDescending(f => request.Orderby);
            }

            if (!query.Any())
            {
                response.Message=HttpStatusCode.NotFound.ToString();
                response.Message = "No Flights Available";
            }
            var flights = query.ToList();
            var flightsdto = new List<FlightDto>();

            foreach (var flight in flights)
            {
                flightsdto.Add(new FlightDto
                {
                    Id = flight.Id,
                    FlightNumber = flight.FlightNumber,
                    Airline = flight.Airline,
                    DepartureCity = flight.DepartureLocation,
                    DepartureTime = flight.DepartureTime.ToString("t"),
                    ArrivalLocation = flight.ArrivalLocation,
                    ArrivalTime = flight.ArrivalTime.ToString("t"),
                    Duration = flight.CalculateDuration(flight.DepartureTime, flight.ArrivalTime),
                    Price = flight.Price,
                });
            }

            response.Data = flightsdto;
            return response;
        }


    }
}
