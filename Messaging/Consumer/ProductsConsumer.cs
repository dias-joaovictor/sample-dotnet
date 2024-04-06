using Confluent.Kafka;

public class ProductsConsumer : BackgroundService
{
    private readonly string _topic = "Products";
    private readonly ILogger<ProductsConsumer> _logger;
    private readonly ConsumerConfig _consumerConfig;

    private readonly ProductService _productService;
    
    private readonly IConsumer<Ignore, string> _consumer;

    public ProductsConsumer(ILogger<ProductsConsumer> logger, ConsumerConfig consumerConfig, ProductService productService)
    {
        _logger = logger;
        _consumerConfig = consumerConfig;
        _productService = productService;
        
        _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        _consumer.Subscribe(_topic);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        
        _logger.LogInformation("ProductsConsumer started consuming.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var product = System.Text.Json.JsonSerializer.Deserialize<Product>(
                    consumeResult.Message.Value, 
                    new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                _logger.LogInformation($"Received product: {product}");

                if(product != null){
                    _productService.addProduct(product);
                }
                

                // Process the product record here
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error consuming message: {ex.Message}");
            }
        }

        _consumer.Close();
    }

    public override void Dispose()
    {
        _consumer.Dispose();
        base.Dispose();
    }
}
