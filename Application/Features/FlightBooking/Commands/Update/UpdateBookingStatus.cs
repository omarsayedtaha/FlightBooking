using Application.Common;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.IO;
using System.Threading.Tasks;
public class UpdateBookingStatus
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateBookingStatus(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponse<string>> Update(string sessionId)
    {
        try
        {

            var Service = new SessionService();
            var session = await Service.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);
                if (payment != null)
                {
                    payment.Status = "Succeeded";
                    _dbContext.Payments.Update(payment);
                }

                var bookingId = payment.BookingId;
                var booking = await _dbContext.FlightBookings.FindAsync(bookingId);
                if (booking != null)
                {
                    booking.Status = "Confirmed";
                    _dbContext.FlightBookings.Update(booking);
                }

                await _dbContext.SaveChangesAsync();

                var PaymentStatus = _dbContext.Payments.FirstOrDefault(x => x.Id == payment.Id).Status;
                var bookingstatus = _dbContext.FlightBookings.FirstOrDefault(x => x.Id == payment.BookingId).Status;

                return new BaseResponse<string>(System.Net.HttpStatusCode.OK, "", $"Payment {PaymentStatus} - Booking {bookingstatus}");
            }

            return new BaseResponse<string>(System.Net.HttpStatusCode.OK, "", "Payment Pending");

        }
        catch (Exception e)
        {
            return new BaseResponse<string>(System.Net.HttpStatusCode.BadRequest, $"{e.Message}", string.Empty);
        }


    }
}