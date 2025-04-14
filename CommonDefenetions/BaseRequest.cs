using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommonDefenitions
{
    public class BaseRequest
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string Orderby { get; set; }
        public bool IsAscending { get; set; }

    }

}
