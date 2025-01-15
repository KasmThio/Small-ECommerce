namespace Small_E_Commerce.Application;

public interface IAuthService
{
    Task<AdminAuthenticationResponse> AdminLoginAsync(string email, string password,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResponse> CustomerLoginAsync(string email, string password,
        CancellationToken cancellationToken = default);

    Task CreateAdminAsync(string email, string password, string role,
        IEnumerable<long>? permissionsIds,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResponse> SingUpAsync(string email, string password,
        CancellationToken cancellationToken = default);
}