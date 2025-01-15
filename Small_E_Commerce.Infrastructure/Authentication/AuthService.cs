using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Small_E_Commerce.Application;
using Small_E_Commerce.Identity;
using Small_E_Commerce.Identity.Users;
using Small_E_Commerce.Identity.Users.UserPermissions;
using Small_E_Commerce.Products;


namespace Small_E_Commerce.Infrastructure.Authentication;

public class AuthService(
    UserManager<User> userManager,
    RoleManager<UserRole> roleManager,
    IUserSessionStore userSessionStore,
    IdentityDbContext identityDbContext,
   IOptions<AuthSettings> authSettings) : IAuthService
{
    public async Task<AdminAuthenticationResponse> AdminLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await identityDbContext
            .Users.Include(user => user.Role)
            .FirstOrDefaultAsync(u => u.Email == email && u.IsAdmin, cancellationToken);
        if (user == null)
        {
           new DomainException("Invalid email or password");
        }

        var result = await userManager.CheckPasswordAsync(user, password);
        if (!result)
        {
            new DomainException("Invalid email or password");
        }

        if (!await userManager.IsInRoleAsync(user, "Admin"))
        {
            new DomainException("Invalid email or password");
        }
        
        var userRoles = await userManager.GetRolesAsync(user);

        var userRole = userRoles.FirstOrDefault();

        if (userRole is null or Roles.Customer)
        {
            throw new DomainException("Only admins are allowed to login to the admin panel");
        }

        var sessionId = Guid.NewGuid().ToString();
        var userSession = new UserSession(user.Id,
            sessionId,
            DateTimeOffset.UtcNow);
        
        var userPermissions = await identityDbContext
            .Set<UserPermission>()
            .Where(x => x.UserId == user.Id)
            .ToArrayAsync(cancellationToken);

        var permissions = Permission.All
            .Where(x => userPermissions.Any(y => y.PermissionId == x.Id))
            .ToArray();

        
        var accessToken = await GenerateAccessToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        
        await userSessionStore.CreateAsync(userSession, cancellationToken);
        
        return new AdminAuthenticationResponse(accessToken, refreshToken, user.Role.Name, user.Id, user.Email, permissions);
    }

    public async Task<AuthenticationResponse> CustomerLoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await identityDbContext
            .Users.Include(user => user.Role)
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsAdmin, cancellationToken);
        if (user == null)
        {
            new DomainException("Invalid email or password");
        }

        var result = await userManager.CheckPasswordAsync(user, password);
        if (!result)
        {
            new DomainException("Invalid email or password");
        }

        if (!await userManager.IsInRoleAsync(user, "Customer"))
        {
            new DomainException("Invalid email or password");
        }
        
        var userRoles = await userManager.GetRolesAsync(user);

        var userRole = userRoles.FirstOrDefault();

        if (userRole is null or Roles.Admin)
        {
            throw new DomainException("Only customers are allowed to login to the app");
        }

        var sessionId = Guid.NewGuid().ToString();
        var userSession = new UserSession(user.Id,
            sessionId,
            DateTimeOffset.UtcNow);
        
        
        var accessToken = await GenerateAccessToken(user);
        var refreshToken = Guid.NewGuid().ToString();
        
        await userSessionStore.CreateAsync(userSession, cancellationToken);
        
        return new AuthenticationResponse(accessToken, refreshToken);
    }

    public async Task CreateAdminAsync(string email, string password, string role,
        IEnumerable<long>? permissionsIds,
        CancellationToken cancellationToken = default)
    {
        var newUser = new User(new Email(email),DateTimeOffset.UtcNow, true);
        var result = await userManager.CreateAsync(newUser, password);

        if (!result.Succeeded)
        {
            throw new DomainException(result.Errors.First().Description);
        }

        var userRoleExists = await roleManager.RoleExistsAsync(role);

        if (!userRoleExists)
        {
            await roleManager.CreateAsync(new UserRole(role));
        }

        await userManager.AddToRoleAsync(newUser, role);

        var permissions = DefaultPermissions.GetPermissionsFor(role);

        if (permissionsIds is not null && permissionsIds.Any())
        {
            var userPermissions = Permission.All
                .Where(x => permissionsIds.Contains(x.Id))
                .Select(x => new UserPermission(x.Id));

            permissions = userPermissions;
        }
        newUser.GrantPermissions(permissions);
        newUser.GrantRole(new UserRole(role));
        
        await userManager.UpdateAsync(newUser);
        await identityDbContext.SaveChangesAsync(cancellationToken);

        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
        await userManager.ConfirmEmailAsync(newUser, confirmationToken);
    }

    public async Task<AuthenticationResponse> SingUpAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var newUser = new User(new Email(email), DateTimeOffset.UtcNow);
        var result = await userManager.CreateAsync(newUser, password);

        if (!result.Succeeded)
        {
            throw new DomainException(result.Errors.First().Description);
        }

        var userRoleExists = await roleManager.RoleExistsAsync(Roles.Customer);

        if (!userRoleExists)
        {
            await roleManager.CreateAsync(new UserRole(Roles.Customer));
        }

        await userManager.AddToRoleAsync(newUser, Roles.Customer);
        
        var sessionId = Guid.NewGuid().ToString();
        var userSession = new UserSession(newUser.Id,
            sessionId,
            DateTimeOffset.UtcNow);
        
        var permissions = DefaultPermissions.GetPermissionsFor(Roles.Customer);

        newUser.GrantPermissions(permissions);
        newUser.GrantRole(new UserRole(Roles.Customer));
        
        var accessToken = await GenerateAccessToken(newUser);
        var refreshToken = Guid.NewGuid().ToString();
        
        await userSessionStore.CreateAsync(userSession, cancellationToken);
        
        return new AuthenticationResponse(accessToken, refreshToken);
    }
    
    private async Task<string> GenerateAccessToken(User user)
    {
        var roles = (await userManager.GetRolesAsync(user)).ToList();

        var jwtPayload = new JwtPayload
        {
            {
                JwtRegisteredClaimNames.Iss,
                authSettings.Value.Issuer
            },
            {
                JwtRegisteredClaimNames.Aud,
                authSettings.Value.Audience
            },
            {
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()
            },
            {
                JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.AddSeconds(-1).ToUnixTimeSeconds()
            },
            {
                JwtRegisteredClaimNames.Email,
                user.Email
            },
            {
                JwtRegisteredClaimNames.Exp,
                DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds()
            },
            {
                "roles",
                roles
            }
        };

        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Value.Key));
        var jwtHeader = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        var securityToken = new JwtSecurityToken(jwtHeader, jwtPayload);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}