namespace Application.Exceptions.ErrorDetails
{
    public class ValidationErrorDetails : ResponseErrorDetails
    {
        public List<ValidationError> ValidationSummary { get; set; }
    }
}
