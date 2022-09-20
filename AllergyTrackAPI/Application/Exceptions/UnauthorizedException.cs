using System.Net;

namespace Application.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public HttpStatusCode StatusCode { get; set; }

        public UnauthorizedException(string message = "Unauthorized") : base(message)
        {
            StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}
