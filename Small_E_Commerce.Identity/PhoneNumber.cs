using System.Data;
using System.Text.RegularExpressions;
using Dawn;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Identity;

public record PhoneNumber : IValueObject
{
    private readonly static Dictionary<Regex, Func<string, string>> _normalization = new Dictionary<Regex, Func<string, string>>
    {
        {new Regex("^(7)"), s => s },
        {new Regex("^(\\+9647)"), s => s.Substring(4) },
        {new Regex("^(009647)"), s => s.Substring(5) },
        {new Regex("^(07)"), s => s.Substring(1) }
    };

    public static readonly Regex _phoneNumberVerifier = new Regex("^(\\+9647|009647|07|7)\\d{9}$");
    public string Number { get; private set; }
    public PhoneNumber(string number)
    {
        Guard.Argument(number, nameof(number)).NotNull().NotEmpty().NotWhiteSpace();
        var cleansedNumber = number.Replace(" ", string.Empty).Trim();
        if (!_phoneNumberVerifier.IsMatch(cleansedNumber))
        {
            throw new DataException($"{number} is not a valid/known iraqi phone number");
        }
        this.Number = Normalize(cleansedNumber);
    }

    private static string Normalize(string cleansedNumber)
    {
        foreach (var schema in _normalization)
        {
            if (schema.Key.IsMatch(cleansedNumber))
            {
                return schema.Value.Invoke(cleansedNumber);
            }
        }

        throw new DataException($"Supplied phone number is not valid or not cleansed. Only valid cleansed phone numbers can be normalized.");
    }
}