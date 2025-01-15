namespace Small_E_Commerce.Internals;

public interface IIdentityProvider
{
    Guid? CurrentUser { get; }
    string? Email { get; }
}