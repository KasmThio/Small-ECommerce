using Microsoft.AspNetCore.Identity;

namespace Small_E_Commerce.Identity.Users;

public class UserRole : IdentityRole<Guid>
{
    public UserRole(string name)
    {
        this.Name = name;
    }
    
    private UserRole(){}
}