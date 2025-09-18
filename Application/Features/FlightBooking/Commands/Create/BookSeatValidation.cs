using FluentValidation;

public class BookSeatValidation : AbstractValidator<SeatBookingRequest>
{
    public BookSeatValidation()
    {
        RuleFor(x => x.flightId).NotEqual(0).WithMessage("flight Id can't be 0");

        RuleFor(x => x.seatNumber).NotNull().WithMessage("seat number can't be null");

    }
}