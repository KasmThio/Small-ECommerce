using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Orders;

public class Address : IValueObject
{
    public string Longitude { get; }
    public string Latitude { get; }
    public string City { get; }
    public string FullAddress { get; }
    
    public Address(string longitude, string latitude, string city, string fullAddress)
    {
        Longitude = longitude;
        Latitude = latitude;
        City = city;
        FullAddress = fullAddress;
    }
}
