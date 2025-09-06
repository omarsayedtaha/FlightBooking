using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class BaseRequest<T> : BaseRequest
    {
        public T? Filter { get; set; }
    }

    public class BaseRequest
    {
        public string? Search { get; set; }
        public string? Orderby { get; set; }

        public bool? IsAscending { get; set; }

        public int? PageIndex { get; set; } = 1;

        public int? PageSize { get; set; } = 10;

    }
}
