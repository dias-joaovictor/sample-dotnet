using Quartz;

public class ProductsTaskProductionTask : IJob
{

    private readonly ILogger<ProductsTaskProductionTask> _logger;

    private readonly ProductsProducer _productsProducer;

    public ProductsTaskProductionTask(ILogger<ProductsTaskProductionTask> logger, ProductsProducer productsProducer) 
    {
        _logger = logger;
        _productsProducer = productsProducer;
    }

    public Task Execute(IJobExecutionContext context)
    {
        Product product = new Product(
            Guid.NewGuid().ToString(), 
            GenerateRandomString(100), 
            new Random().Next(0, 100000)/100m);
        _logger.LogInformation("Producing Product " + product);
        return _productsProducer.ProduceAsync(product);
    }


    private static string GenerateRandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}