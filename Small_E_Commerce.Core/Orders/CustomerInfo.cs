using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Orders;

public class CustomerInfo : IValueObject
{
    public Guid Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string PhoneNumber { get; }
    
    public CustomerInfo(Guid id, string name, string email, string phoneNumber)
    {
        Id = id;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    
    private CustomerInfo(){}
    
}