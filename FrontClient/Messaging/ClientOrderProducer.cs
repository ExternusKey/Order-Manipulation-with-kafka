﻿using System.Text.Json;
using Confluent.Kafka;
using ServicesManipulation.Models;

namespace ServicesManipulation.Messaging;

public class ClientOrderProducer
{
    private readonly IProducer<string?, string> _producer;
    private const string TopicName = "OrderQueue";

    public ClientOrderProducer()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:29092",
        };
        
        _producer = new ProducerBuilder<string?, string>(config).Build();
    }

    public async Task<string?> SendOrderAsync(OrderRequest order)
    {
        var message = new Message<string?, string>
        {
            Key = order.OrderId,
            Value = JsonSerializer.Serialize(order)
        };

        try
        {
            await _producer.ProduceAsync(TopicName, message);
            Console.WriteLine($"[Web-Client] New order send to kafka : {message.Key} в {TopicName}");
            return order.OrderId;
        }
        catch (ProduceException<string, string> e)
        {
            Console.WriteLine(e);
            throw new Exception($"[Web-Client] Error sending order to kafka : {e.Message}]");
        }
    }
    
}