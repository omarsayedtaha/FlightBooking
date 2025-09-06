using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Flight
{
    public class FlightRequest
    {
        public Guid? Id { get; set; }

        public DateTime Date { get; set; }
        public string from { get; set; }

        public string to { get; set; }


    }
}
