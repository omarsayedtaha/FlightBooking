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


        [HttpGet("getAllflights")]
        public async Task<IActionResult> GetAll()
        {
            var Service = new GetAllFlights(_context , _request);
            var flights = Service.GetAll();
            return Ok(flights);
        }

        [HttpGet("getflightswithfilter")]
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