using Application.Common;
using Application.Dtos.Flight;
using Application.Dtos.Seats.SeatDto;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

public class GetFlightWithId
{
    private readonly IApplicationDbContext _dbContext;

    public GetFlightWithId(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<FlightDto>> GetWithId(int flightId)
    {
        if (flightId == 0)
            return new BaseResponse<FlightDto>(System.Net.HttpStatusCode.BadRequest, "Bad Request", null);

        var flight = _dbContext.Flights.Include(x => x.Seats)
         .FirstOrDefault(x => x.Id == flightId);

        var flightDto = new FlightDto()
        {
            Id = flight.Id,
            Airline = flight.Airline,
            DepartureLocation = flight.DepartureLocation,
            DepartureDate = flight.DepartureTime.ToString("d"),
            DepartureTime = flight.DepartureTime.ToString("t"),
            ArrivalLocation = flight.ArrivalLocation,
            ArrivalDate = flight.ArrivalTime.ToString("d"),
            ArrivalTime = flight.ArrivalTime.ToString("t"),
            Duration = flight.CalculateDuration(flight.DepartureTime, flight.ArrivalTime),
            FlightNumber = flight.FlightNumber,
            Seats = flight.Seats.Select(f => new SeatDto { SeatNumber = f.SeatNumber, IsBooked = f.IsBooked }).ToList()
        };

        return new BaseResponse<FlightDto>(System.Net.HttpStatusCode.OK, "", flightDto);
    }
}