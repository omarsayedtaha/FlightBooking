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

        private readonly CreateFlight createFlight;
        private readonly UpdateFlight updateFlight;
        private readonly GetFlightsWithFilter getFlightsWithFilter;
        private readonly GetFlightWithId getFlightWithId;

        public FlightController(
         CreateFlight createFlight,
          UpdateFlight updateFlight,
          GetFlightsWithFilter getFlightsWithFilter,
          GetFlightWithId getFlightWithId)
        {

            this.createFlight = createFlight;
            this.updateFlight = updateFlight;
            this.getFlightsWithFilter = getFlightsWithFilter;
            this.getFlightWithId = getFlightWithId;
        }

        [HttpPost("create-flight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateFlightDto model)
        {
            var result = await createFlight.Create(model);

            return Ok(result);
        }

        [HttpPut("update-flight")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateFlightDto model)
        {
            var result = await updateFlight.Update(model);

            return Ok(result);
        }

        [HttpGet("get-flights-with-filter")]
        public async Task<IActionResult> GetWithFilter([FromQuery] BaseRequest<FlightRequest> flightFilter)
        {
            var flights = await getFlightsWithFilter.Get(flightFilter);
            return Ok(flights);
        }

        [HttpGet("get-flights-with-Id/{Id}")]
        public async Task<IActionResult> GetWithId([FromRoute] int Id)
        {
            var flights = await getFlightWithId.GetWithId(Id);
            return Ok(flights);
        }


    }
}