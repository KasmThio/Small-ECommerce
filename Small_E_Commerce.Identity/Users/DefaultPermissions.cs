using Small_E_Commerce.Identity.Users.UserPermissions;

namespace Small_E_Commerce.Identity.Users;

public class DefaultPermissions
{
    public static IEnumerable<UserPermission> GetPermissionsFor(string role)
    { 
        var allPermissions = Permission.All;
        
        return role switch
        {
            Roles.SuperAdmin => allPermissions.Select(p => new UserPermission(p.Id)).ToArray(),
            Roles.Admin => new UserPermission[]
            {
                new(allPermissions.First(x => x.Name == "Order.AdminView").Id),
                new(allPermissions.First(x => x.Name == "Product.AdminView").Id),
                new(allPermissions.First(x => x.Name == "Order.Create").Id),
                new(allPermissions.First(x => x.Name == "Product.Create").Id),
                new(allPermissions.First(x => x.Name == "Order.Update").Id),
                new(allPermissions.First(x => x.Name == "Product.Update").Id),
                new(allPermissions.First(x => x.Name == "Order.Delete").Id),
                new(allPermissions.First(x => x.Name == "Product.Delete").Id),
                new(allPermissions.First(x => x.Name == "Report.OrdersWeekly").Id),
            },
            Roles.Customer => new UserPermission[]
            {
                new(allPermissions.First(x => x.Name == "Order.View").Id),
                new(allPermissions.First(x => x.Name == "Product.View").Id),
                new(allPermissions.First(x => x.Name == "Order.Create").Id),
            },
            _ => throw new ArgumentException("Invalid role")
        };
    }
}
