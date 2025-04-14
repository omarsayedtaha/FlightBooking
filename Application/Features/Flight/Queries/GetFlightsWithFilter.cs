using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDefenitions;
using CommonDefenitions.Dtos.Flight;
using Infrastructure;

namespace Application.Features.Flight.Queries
{
    public class GetFlightsWithFilter
    {
        private readonly ApplicationDbContext _context;

        public GetFlightsWithFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        //public Task<BaseResponse<Domain.Flight>> Get(BaseRequest request , FlightFilterDto filter)
        //{
        //    if (filter!=null)
        //    {
        //        if (filter.Id!=null|| !)
        //        {
        //           var query =  _context.Flights.Where(f=>f.Id==filter.Id ||)
        //        }
        //    }
        //}
    }
}
