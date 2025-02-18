using System.Text.Json;
using Confluent.Kafka;
using DbClientService.Data;
using DbClientService.Models;

namespace DbClientService.Messaging;

public class ServerOrderConfirmationProducer
{
    private readonly IProducer<string, string> _producer;
    private const string TopicName = "ConfirmationQueue";

    public ServerOrderConfirmationProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:29092",
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendOrderConfirmationAsync(OrderConfirmation orderConfirmation)
    {
        var message = new Message<string, string>
        {
            Key = orderConfirmation.OrderId,
            Value = JsonSerializer.Serialize(orderConfirmation)
        };
        
        await _producer.ProduceAsync(TopicName, message);
        Console.WriteLine("[Back-Client] OrderConfirmation send to Kafka...");
    }
}