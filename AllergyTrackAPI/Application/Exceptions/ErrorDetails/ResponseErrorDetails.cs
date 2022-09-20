using Newtonsoft.Json;

namespace Application.Exceptions.ErrorDetails
{
    public class ResponseErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
