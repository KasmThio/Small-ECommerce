using System.Runtime.Serialization;

namespace Small_E_Commerce;

[Serializable]
public class IllegalStateTransitionException : Exception
{
    public IllegalStateTransitionException()
    {
    }

    public IllegalStateTransitionException(string? message) : base(message)
    {
    }

    public IllegalStateTransitionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected IllegalStateTransitionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}