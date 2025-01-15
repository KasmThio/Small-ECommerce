using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Products;

public class ProductOptions : IValueObject
{
    public string Name { get; } // e.g. Size, Color, etc.
    public string Value{ get; } // e.g. Small, Red, etc.
    
    public ProductOptions(string name, string value)
    {
        Name = name;
        Value = value;
    }
}