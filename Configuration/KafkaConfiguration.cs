using Confluent.Kafka;

public class KafkaConfiguration {

    public static void ConfigureKafka(WebApplicationBuilder builder) {
        InjectConsumerConfig(builder);
        ConfigureConsumers(builder);
        ConfigureProducers(builder);
    }

    private static void ConfigureConsumers(WebApplicationBuilder builder) {
        // Include Products Consumer
        builder.Services.AddHostedService<ProductsConsumer>();
    }

    private static void ConfigureProducers(WebApplicationBuilder builder) {
        builder.Services.AddSingleton(new ProductsProducer("localhost:9092"));
    }

    private static void InjectConsumerConfig(WebApplicationBuilder builder) {
        var kafkaConsumerConfig = new ConsumerConfig();
        builder.Configuration.GetSection("KafkaConsumerConfig").Bind(kafkaConsumerConfig);
        builder.Services.AddSingleton(kafkaConsumerConfig);
    }

}