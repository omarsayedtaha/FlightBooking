using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {

        }
        public BaseResponse(HttpStatusCode statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }

        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class PaginatedResponse<T> : BaseResponse<T>
    {
        public PaginatedResponse()
        {

        }
        public PaginatedResponse(HttpStatusCode statusCode, string message, T data, int pageSize, int pageIndex, int count)
        : base(statusCode, message, data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
    }
}
