using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Seat.Controllers
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

        // public async Task<> GetSeatByNumber(string Number)
        // {
        //     var service = new GetSeatByNubmer(_context);
        //     var result = service.
        // }
    }
}