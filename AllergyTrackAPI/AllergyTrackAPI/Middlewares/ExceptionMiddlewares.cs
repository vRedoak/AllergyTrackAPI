using Application.Exceptions;
using Application.Exceptions.ErrorDetails;
using FluentValidation;

namespace AllergyTrackAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"An error occurred: {exception}");
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ResponseErrorDetails errorDetails;

            switch (exception)
            {
                case ValidationException validationException:
                    {
                        errorDetails = new ValidationErrorDetails()
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = validationException.Message,
                            ValidationSummary = validationException.Errors.Select(valRes =>
                                        new ValidationError()
                                        {
                                            FieldName = valRes.PropertyName,
                                            Message = valRes.ErrorMessage
                                        }
                                    ).ToList()
                        };

                        break;
                    }

                case ApiException apiException:
                    {
                        errorDetails = new ResponseErrorDetails()
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = apiException.Message
                        };
                        break;
                    }

                case UnauthorizedException unauthorizedException:
                    {
                        errorDetails = new ResponseErrorDetails()
                        {
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = unauthorizedException.Message
                        };
                        break;
                    }

                default:
                    {
                        errorDetails = new ResponseErrorDetails()
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = exception.ToString(),
                        };

                        break;
                    }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorDetails.StatusCode;

            return context.Response.WriteAsync(errorDetails.ToString());
        }
    }
}
