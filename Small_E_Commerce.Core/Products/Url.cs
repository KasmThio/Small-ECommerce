using System.Data;
using Dawn;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Products;

public class Url : IValueObject
{
    public string Value { get; set; }
    
    public Url(string value)
    {
        Guard.Argument(value).NotNull().NotEmpty();
        
        if (!IsValidUrl(value))
        {
            throw new DataException("Invalid URL format");
        }
        
        Value = value;
    }
    
    private bool IsValidUrl(string url)
    {
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        {
            return false;
        }

        try
        {
            var uri = new Uri(url);
            return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
        }
        catch (UriFormatException)
        {
            return false;
        }
    }
    
    private Url(){}
}
