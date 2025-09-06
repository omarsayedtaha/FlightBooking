using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Dtos.Flight;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Features.Flight.Queries
{
    public class GetFlightsWithFilter : BaseServices<Domain.Flight, FlightRequest>
    {
        private readonly IApplicationDbContext _context;

        public GetFlightsWithFilter(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResponse<IEnumerable<FlightDto>>> Get(BaseRequest<FlightRequest> request)
        {
            PaginatedResponse<IEnumerable<FlightDto>> response = new PaginatedResponse<IEnumerable<FlightDto>>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = null;

            var query = _context.Flights.AsQueryable().AsNoTracking();

            if (request.Filter != null)
                query = ApplyFilter(query, request);

            if (!string.IsNullOrEmpty(request.Search))
                query = ApplySearch(query, request.Search);

            if (!string.IsNullOrEmpty(request.Orderby))
            {
                if (request.Orderby.ToLower() == "Price".ToLower())
                {
                    query = request.IsAscending.Value ?
                    query.OrderBy(f => f.Price)
                  : query.OrderByDescending(f => f.Price);
                }
            }

            // Only flights departing in ≤ 1 hour and with available seats
            // query = query.Where(f =>
            //     f.NumberOfSeatsAvialable < f.NumberOfSeats &&
            //     f.DepartureTime > DateTime.Now &&
            //     f.DepartureTime <= DateTime.Now.AddHours(1));

            var flights = await query.ToListAsync();

            if (!flights.Any())
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "No Flights Available";
            }

            response.Data = flights.Select(f => new FlightDto()
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

        public override IQueryable<Domain.Flight> ApplyFilter(IQueryable<Domain.Flight> query, BaseRequest<FlightRequest> request)
        {
            if (request.Filter.Id.HasValue)
            {
                query = query.Where(f => f.Id == request.Filter.Id.Value);

            }
            if (request.Filter.Date > DateTime.MinValue)
            {

                query = query.Where(f => f.DepartureTime.Date == request.Filter.Date.Date/*.ToDateTime(TimeOnly.MinValue)*/);

            }
            if (!string.IsNullOrEmpty(request.Filter.from))
            {
                query = query.Where(f => f.DepartureLocation.ToLower().Contains(request.Filter.from.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Filter.to))
            {
                query = query.Where(f => f.ArrivalLocation.ToLower().Contains(request.Filter.to.ToLower()));
            }
            return query;
        }

        public override IQueryable<Domain.Flight> ApplySearch(IQueryable<Domain.Flight> query, string search)
        {

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f => f.Airline == search);
            }
            return query;
        }

        public override IQueryable<Domain.Flight> ApplyPagination(IQueryable<Domain.Flight> query, int PageIndex, int PageSize)
        {
            if (PageIndex > 1)
                query = query.Skip(PageSize * (PageIndex - 1)).Take(PageSize);
            else
                query = query.Take(PageSize);

            return query;
        }
    }
}
