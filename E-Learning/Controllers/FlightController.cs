using CommonDefenitions;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[HttpGet("getflightswithfilter")]
        //public async Task<IActionResult> Get(BaseRequest request)
        //{
           
        //}
    }
}
