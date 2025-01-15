using System.Runtime.Serialization;

namespace Small_E_Commerce.Products;

[Serializable]
public class DomainException : Exception
{
    public DomainException()
    {
    }

    public DomainException(string? message) : base(message)
    {
    }

    public DomainException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DomainException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}