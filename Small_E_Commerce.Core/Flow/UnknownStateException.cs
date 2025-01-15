using System.Runtime.Serialization;

namespace Small_E_Commerce;

[Serializable]
public class UnknownStateException : Exception
{
    public UnknownStateException()
    {
    }

    public UnknownStateException(string? message) : base(message)
    {
    }

    public UnknownStateException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected UnknownStateException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}