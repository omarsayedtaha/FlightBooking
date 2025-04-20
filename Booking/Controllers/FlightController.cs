using Application.Features.Flight.Commands.Create;
using Application.Features.Flight.Commands.Update;
using Application.Features.Flight.Queries;
using CommonDefenitions;
using CommonDefenitions.Dtos.Flight;
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
        private readonly ApplicationDbContext _context;
        private readonly BaseRequest _request;

        public FlightController(ApplicationDbContext context , BaseRequest request)
        {
            _context = context;
            _request = request;
        }

        [HttpPost("create-flight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody]CreateFlightDto model)
        {
            var CreateFlightService = new CreateFlight(_context);
          var result = await CreateFlightService.Create(model);

            return Ok(result);
        }

        [HttpPut("update-flight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromQuery]Guid Id,[FromBody]CreateFlightDto model)
        {
            var UpdateFlightService = new UpdateFlight(_context);
            var result = await UpdateFlightService.Update(Id,model);

            return Ok(result);
        }

        [HttpGet("get-flights-with-filter")]
        public async Task<IActionResult> GetWithFilter([FromQuery]BaseRequest<FlightRequest> flightFilter)
        {
            var Service = new GetFlightsWithFilter(_context);
            var flights = Service.Get(flightFilter);
            return Ok(flights); 
        }

        //[HttpGet("Test")]
        //public async Task<IActionResult> Test()
        //{

        //    return Ok("working.......");
        //}

    }
}