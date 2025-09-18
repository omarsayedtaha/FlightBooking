using System.Diagnostics;
using System.Runtime.CompilerServices;
using Application.Common;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domian.Enums;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class BookSeat
{
    private readonly IApplicationDbContext _dbContext;
    private readonly AppHelperSerivices _appHelper;
    private readonly UserManager<User> _userManager;

    public BookSeat(IApplicationDbContext dbContext, AppHelperSerivices appHelper, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _appHelper = appHelper;
        _userManager = userManager;
    }

    public async Task<BaseResponse<string>> Book(SeatBookingRequest request)
    {
        var validation = new BookSeatValidation();
        var result = validation.Validate(request);
        if (!result.IsValid)
        {
            var ErrorMessage = string.Join(",", result.Errors.Select(x => x.ErrorMessage));
            return new BaseResponse<string>(System.Net.HttpStatusCode.BadRequest, ErrorMessage, string.Empty);
        }
        var seat = new Seat();
        seat = await _dbContext.Seats.Include(s => s.Flight)
        .Include(s => s.SeatClass)
        .FirstOrDefaultAsync(x => x.SeatNumber == request.seatNumber && x.FlightId == request.flightId);

        if (seat == null)
            return new BaseResponse<string>(System.Net.HttpStatusCode.NotFound, "seat not found", string.Empty);

        if (seat.IsBooked)
            return new BaseResponse<string>(System.Net.HttpStatusCode.Conflict, "This Seat is already boked", string.Empty);

        var userId = await _appHelper.GetUserIdAsync();
        var user = await _userManager.FindByIdAsync(userId);
        if (_dbContext.Bookings.Any(u => u.UserId == userId))
        {
            var userbooking = _dbContext.Bookings.Include(x => x.FlightBookings).FirstOrDefault(u => u.UserId == userId);

            if (userbooking == null)
                return new BaseResponse<string>(System.Net.HttpStatusCode.NotFound, "user has no booking", string.Empty);

            if (userbooking.FlightBookings.Last().ArrivalDate > seat.Flight.DepartureTime)
                return new BaseResponse<string>(System.Net.HttpStatusCode.Conflict, "flight Intervals Overlap", string.Empty);
        }

        var booking = new Booking
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            Status = BookingStatus.Pending.ToString(),
            TravelDate = seat.Flight.DepartureTime

        };


        _dbContext.Bookings.Add(booking);
        await _dbContext.SaveChangesAsync();

        var flightBooking = new FlightBookings
        {
            FlightId = seat.Flight.Id,
            BookingId = booking.Id,
            SeatId = seat.Id,
            DepartureDate = seat.Flight.DepartureTime,
            ArrivalDate = seat.Flight.ArrivalTime,
            Price = seat.SeatClass.Price,
            CreatedAt = DateTime.Now,
            Status = BookingStatus.Pending.ToString(),
            PassengerName = $"{user.FirstName} {user.LastName}",
            PassportNumber = user.PassportNumber,
        };

        _dbContext.FlightBookings.Add(flightBooking);
        await _dbContext.SaveChangesAsync();

        return new BaseResponse<string>(System.Net.HttpStatusCode.OK, "", $"book is {booking.Status}");
    }


}