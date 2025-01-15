namespace Small_E_Commerce.Identity.Users.UserPermissions;

public readonly record struct Permission(int Id, string Name, string Description)
{
    public static readonly HashSet<Permission> All = new HashSet<Permission>()
    {
        new Permission(1, Permissions.All.Permissions, "All permissions"),
        new Permission(2, "View.Users", "View users"),
        new Permission(3, "Create.Users", "Create users"),
        new Permission(4, "Update.Users", "Update users"),
        new Permission(5, "Delete.Users", "Delete users"),
        new Permission(6, "Order.Create", "Create order"),
        new Permission(7, "Order.View", "View order"),
        new Permission(8, "Order.Update", "Update order"),
        new Permission(9, "Product.Create", "Create product"),
        new Permission(10, "Product.View", "View product"),
        new Permission(11, "Product.Update", "Update product"),
        new Permission(12, "Product.Delete", "Delete product"),
        new Permission(13, "Product.AdminView", "Admin view product"),
        new Permission(14, "Order.AdminView", "Admin view order"),
        new Permission(15, "Report.OrdersWeekly", "Weekly order report"),
    };
}

public static class Permissions
{
    public static class All
    {
        public const string Permissions = "All";
    }
}
