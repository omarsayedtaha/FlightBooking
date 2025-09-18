using Application.Common;
using Application.Dtos.Flight;
using Application.Features.Flight.Commands.Create;
using Application.Features.Flight.Commands.Update;
using Application.Features.Flight.Queries;
using Application.Interfaces;
using CommonDefenitions;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly BaseRequest _request;

        public FlightController(IApplicationDbContext context, BaseRequest request)
        {
            _context = context;
            _request = request;
        }

        [HttpPost("create-flight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateFlightDto model)
        {
            var CreateFlightService = new CreateFlight(_context);
            var result = await CreateFlightService.Create(model);

            return Ok(result);
        }

        [HttpPut("update-flight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateFlightDto model)
        {
            var UpdateFlightService = new UpdateFlight(_context);
            var result = await UpdateFlightService.Update(model);

            return Ok(result);
        }

        [HttpGet("get-flights-with-filter")]
        public async Task<IActionResult> GetWithFilter([FromQuery] BaseRequest<FlightRequest> flightFilter)
        {
            var Service = new GetFlightsWithFilter(_context);
            var flights = await Service.Get(flightFilter);
            return Ok(flights);
        }

        [HttpGet("get-flights-with-Id/{Id}")]
        public async Task<IActionResult> GetWithId([FromRoute] int Id)
        {
            var Service = new GetFlightWithId(_context);
            var flights = await Service.GetWithId(Id);
            return Ok(flights);
        }


    }
}