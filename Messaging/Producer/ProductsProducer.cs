using Confluent.Kafka;
using System.Text.Json;
using System.Threading.Tasks;

public class ProductsProducer
{
    private readonly string _bootstrapServers;
    public static readonly string TOPIC = "Products";

    public ProductsProducer(string bootstrapServers)
    {
        _bootstrapServers = bootstrapServers;
    }

    public async Task ProduceAsync(Product product)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using var producer = new ProducerBuilder<Null, string>(config).Build();
        
        try
        {
            var productJson = JsonSerializer.Serialize(product);
            var result = await producer.ProduceAsync(TOPIC, new Message<Null, string> { Value = productJson });
            Console.WriteLine($"Message {result.Value} delivered to {result.TopicPartitionOffset}");
        }
        catch (ProduceException<Null, string> e)
        {
            Console.WriteLine($"Failed to deliver message: {e.Message} [{e.Error.Code}]");
        }
    }
}
