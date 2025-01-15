using Dawn;
using Small_E_Commerce.Internals;

namespace Small_E_Commerce.Products;

public class Product : AggregateBase<Guid>
{
    public Guid? ParentId { get; private set; }
    public string Name { get; private set; }
    public string SKU { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public int LowStockThreshold { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public string Category { get; private set; }
    public Url ImageUrl { get; private set; }
    public ProductOptions ProductOptions { get; private set; }
    public ProductStatus Status { get; private set; }

    public Product(Guid? parentId, string name, string description, decimal price, int stock, string category,
        Url imageUrl, ProductOptions productOptions, int lowStockThreshold, DateTime? expirationDate)
    {
        Guard.Argument(parentId, nameof(parentId)).NotDefault();
        Guard.Argument(name, nameof(name)).NotNull().NotEmpty().MinLength(2);
        Guard.Argument(description, nameof(description)).NotNull().NotEmpty().MinLength(20);
        Guard.Argument(price, nameof(price)).NotNegative();
        Guard.Argument(stock, nameof(stock)).NotNegative();
        Guard.Argument(category, nameof(category)).NotNull().NotEmpty();
        Guard.Argument(imageUrl, nameof(imageUrl)).NotNull();
        Guard.Argument(productOptions, nameof(productOptions)).NotNull();
        Guard.Argument(lowStockThreshold, nameof(lowStockThreshold)).NotNegative();
        Guard.Argument(expirationDate, nameof(expirationDate)).NotDefault().GreaterThan(DateTime.Now);
        
        ParentId = parentId;
        Name = name;
        SKU = Name + DateTimeOffset.Now.ToUnixTimeSeconds();
        Description = description;
        Price = price;
        Stock = stock;
        Category = category;
        ImageUrl = imageUrl;
        ProductOptions = productOptions;
        LowStockThreshold = lowStockThreshold;
        ExpirationDate = expirationDate;
        Status = ProductStatus.Visible;
    }
    
    private Product(){}

    public void UpdateProduct(string? name,
        string? description,
        decimal? price,
        int? stock,
        string? category,
        Url? imageUrl,
        ProductOptions? productOptions,
        int? lowStockThreshold,
        DateTime? expirationDate)
    {
        Guard.Argument(name, nameof(name)).NotEmpty().MinLength(2);
        Guard.Argument(description, nameof(description)).NotEmpty().MinLength(20);
        Guard.Argument(price, nameof(price)).NotNegative();
        Guard.Argument(stock, nameof(stock)).NotNegative();
        Guard.Argument(category, nameof(category)).NotEmpty();
        Guard.Argument(lowStockThreshold, nameof(lowStockThreshold)).NotNegative();
        Guard.Argument(expirationDate, nameof(expirationDate)).NotDefault();
        
        
        Name = name ?? Name;
        Description = description ?? Description;
        Price = price ?? Price;
        Stock = stock ?? Stock;
        Category = category ?? Category;
        ImageUrl = imageUrl ?? ImageUrl;
        ProductOptions = productOptions ?? ProductOptions;
        LowStockThreshold = lowStockThreshold ?? LowStockThreshold;
        ExpirationDate = expirationDate ?? ExpirationDate;
    }
    
    public void DeleteProduct()
    {
        Status = ProductStatus.Deleted;
    }
    
    public void HideProduct()
    {
        Status = ProductStatus.Hidden;
    }
    
    public void ShowProduct()
    {
        Status = ProductStatus.Visible;
    }
    
    public void CheckStock()
    {
        if (Stock <= LowStockThreshold)
        {
            Status = ProductStatus.LowStock;
        }
        
        if (Stock == 0)
        {
            Status = ProductStatus.OutOfStock;
        }
    }
    
    public void CheckExpirationDate()
    {
        if (ExpirationDate < DateTime.Now)
        {
            Status = ProductStatus.Expired;
        }
    }
    
    public void DecreaseStock(int quantity)
    {
        Guard.Argument(quantity, nameof(quantity)).NotNegative();
        
        Stock -= quantity;
        
        CheckStock();
    }
}