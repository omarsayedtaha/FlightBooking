using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CommonDefenitions
{
    public class BaseRequest<T>
    {
        public string? Search { get; set; }
        public string? Orderby { get; set; }

        public bool? IsAscending { get; set; }=true;

        public T? Filter { get; set; }
    }

    public class BaseRequest
    {
        public string? Search { get; set; }
        public string? Orderby { get; set; }

        public bool IsAscending { get; set; }

    }
}
