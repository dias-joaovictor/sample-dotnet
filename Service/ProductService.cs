using System.Collections.Concurrent;


public class ProductService
{
    
    // The static ConcurrentDictionary
    public readonly static ConcurrentDictionary<string, Product> MyProductDb = new ConcurrentDictionary<string, Product>();

    private readonly ILogger<ProductService> _logger;

    public ProductService(ILogger<ProductService> logger){
        _logger = logger;
    }

    public void addProduct(Product product){
        _logger.LogInformation($"Adding product: {product}");
        MyProductDb.TryAdd(product.Id, product);
    }

    public List<Product> getAllProducts() {
        return [.. MyProductDb.Values];
    }
}
