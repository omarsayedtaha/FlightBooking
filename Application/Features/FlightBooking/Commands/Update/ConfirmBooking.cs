using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.services;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domian.Enums;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;



namespace Application.Features.FlightBooking.Commands.Update
{
    public class ConfirmBooking
    {
        private readonly IApplicationDbContext _context;
        private readonly AppHelperSerivices _appHelper;

        public ConfirmBooking(IApplicationDbContext context, AppHelperSerivices appHelper)
        {
            _context = context;
            _appHelper = appHelper;
        }
        public async Task<BaseResponse<PaymentDto>> Confirm()
        {
            var response = new BaseResponse<PaymentDto>();
            response.StatusCode = HttpStatusCode.OK;
            response.Message = string.Empty;
            response.Data = null;

            var user = await _appHelper.GetUserAsync();

            var userbookings = await _context.Bookings.Include(x => x.Payments)
            .Include(x => x.FlightBookings)
            .ThenInclude(f => f.Seat)
            .ThenInclude(s => s.SeatClass)
            .ThenInclude(x => x.Flight)
            .FirstOrDefaultAsync(b => b.UserId == user.Id);

            if (userbookings == null)
            {
                response.Message = "Booking not found";
                return response;
            }
            var paymentsession = await CreatPaymentSession(userbookings);

            var Payment = new Payments()
            {
                BookingId = userbookings.Id,
                CreatedAt = DateTime.Now,
                StripeSessionId = paymentsession.Id,
                Currency = "usd",
                Amount = userbookings.FlightBookings.FirstOrDefault(x => x.BookingId == userbookings.Id).Price,
                Status = PaymentStatus.pending.ToString(),
            };
            _context.Payments.Add(Payment);
            await _context.SaveChangesAsync();

            response.Data = new PaymentDto { Url = paymentsession.Url, SessionId = paymentsession.Id };
            return response;

        }
        public async Task<Session> CreatPaymentSession(Domain.Entities.Booking userbookings)
        {

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = userbookings.FlightBookings.Select(f => new SessionLineItemOptions
                {

                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)f.Price * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Flight: {f.Flight.FlightNumber} - {f.Seat.SeatClass.Class.ToString()}"
                        }
                    },
                    Quantity = 1

                }).ToList(),
                Mode = "payment",
                // SuccessUrl = "https://localhost:7066/success?session_id={CHECKOUT_SESSION_ID}",
                SuccessUrl = "https://localhost:7066/swagger/index.html",
                // CancelUrl = "https://localhost:7066/cancel",
                CancelUrl = "ttps://localhost:7066/swagger/index.html",

            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }

}

