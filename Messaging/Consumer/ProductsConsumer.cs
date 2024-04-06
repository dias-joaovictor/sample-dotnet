using Confluent.Kafka;

public class ProductsConsumer : BackgroundService
{
    private readonly string _topic = "Products";
    private readonly ILogger<ProductsConsumer> _logger;
    private readonly ConsumerConfig _consumerConfig;

    public ProductsConsumer(ILogger<ProductsConsumer> logger, ConsumerConfig consumerConfig)
    {
        _logger = logger;
        _consumerConfig = consumerConfig;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(_topic);
        
        _logger.LogInformation("ProductsConsumer started consuming.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var product = System.Text.Json.JsonSerializer.Deserialize<Product>(
                    consumeResult.Message.Value, 
                    new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                    );
                _logger.LogInformation($"Received product: {product}");

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

        consumer.Close();
    }
}
