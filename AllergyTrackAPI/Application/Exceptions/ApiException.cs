using System.Net;

namespace Application.Exceptions
{
    public class ApiException : ApplicationException
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(string message = "Internal Server Error") : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
    }
}
