using System.Data;
using System.Text.RegularExpressions;
using Dawn;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Identity;

public record Email : IValueObject
{
    private static readonly Regex _emailVerifier = new Regex("^[^@\\s]+@[^@\\s]+\\.[a-zA-Z][a-zA-Z][a-zA-Z]?$");
    public string EmailAddress { get; private set; }
    public Email(string emailAddress)
    {
        Guard.Argument(emailAddress, nameof(emailAddress)).NotNull().NotEmpty().NotWhiteSpace();
        if (!Validate(emailAddress))
        {
            throw new DataException($"{emailAddress} is not a valid email address.");
        }
        this.EmailAddress = emailAddress.ToLower();
    }
    
    public static bool Validate(string emailAddress)
    {
        return _emailVerifier.IsMatch(emailAddress);
    }
}