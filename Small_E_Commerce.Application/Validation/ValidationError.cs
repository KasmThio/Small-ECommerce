namespace Small_E_Commerce.Application.Validation;

public struct ValidationError
{
    public string PropertyName { get; private set; }
    public string ErrorMessage { get; private set; }
    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}