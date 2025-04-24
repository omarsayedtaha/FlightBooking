using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions.Dtos.Flight;
using FluentValidation;

namespace Application.Features.Flight.Commands.Update
{
    public class UpdateFlightValidator:AbstractValidator<UpdateFlightDto>
    {
        public UpdateFlightValidator()
        {
            RuleFor(f => f.FlightId)
                .NotEmpty().WithMessage("Flight ID is required.");

            RuleFor(f => f.FlightNumber)
                .NotEmpty().WithMessage("Flight number is required.")
                .Matches("^[A-Z0-9]{2,10}$").WithMessage("Flight number must be alphanumeric and between 2 to 10 characters.");

            RuleFor(f => f.Airline)
                .NotEmpty().WithMessage("Airline name is required.")
                .MaximumLength(100).WithMessage("Airline name must not exceed 100 characters.");

            RuleFor(f => f.DepartureLocation)
                .NotEmpty().WithMessage("Departure location is required.")
                .MaximumLength(100).WithMessage("Departure location must not exceed 100 characters.");

            RuleFor(f => f.ArrivalLocation)
                .NotEmpty().WithMessage("Arrival location is required.")
                .MaximumLength(100).WithMessage("Arrival location must not exceed 100 characters.");

            RuleFor(f => f.DepartureTime)
                .NotEmpty().WithMessage("Departure time is required.")
                .LessThan(f => f.ArrivalTime).WithMessage("Departure time must be earlier than arrival time.");

            RuleFor(f => f.ArrivalTime)
                .NotEmpty().WithMessage("Arrival time is required.");

            RuleFor(f => f.NumberOfSeats)
                .GreaterThan(0).WithMessage("Number of seats must be greater than 0.")
                .LessThanOrEqualTo(900).WithMessage("Number of seats must not exceed 900.");

            RuleFor(f => f.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}
