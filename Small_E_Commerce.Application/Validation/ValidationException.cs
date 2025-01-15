namespace Small_E_Commerce.Application.Validation;

public class ValidationException : Exception
{
    public List<ValidationError> Errors { get; private set; }

    public ValidationException(List<ValidationError> errors)
    {
        Errors = errors;
    }
}