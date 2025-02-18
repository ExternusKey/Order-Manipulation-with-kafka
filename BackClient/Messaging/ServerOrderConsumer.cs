using System.Text.Json;
using Confluent.Kafka;
using DbClientService.Models;

namespace DbClientService.Messaging;

public class ServerOrderConsumer
{
    private const string TopicName = "OrderQueue";
    private readonly IConsumer<string, string> _consumer;

    public ServerOrderConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "order-consumer",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(TopicName);
    }

    public async Task StartProcessingAsync(CancellationToken cancellationToken)
    {
        var serverConfirmationProducer = new ServerOrderConfirmationProducer();
        
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var order = JsonSerializer.Deserialize<OrderRequest>(consumeResult.Message.Value);
                
                if (order == null) 
                    continue;
                
                Console.WriteLine($"[Back-Client] Order received: {order.OrderId} for {order.UserName}");
                
                // TODO: Запись ордера в таблицу
                await Task.Delay(1000);
                
                var confirmation = new OrderConfirmation
                {
                    OrderId = order.OrderId,
                    UserName = order.UserName,
                    ProcessedBy = "Back-Client"
                };
                
                await serverConfirmationProducer.SendOrderConfirmationAsync(confirmation);
            }
        }
        catch (OperationCanceledException)
        {
            // TODO: LOGGER
        }
        finally
        {
            _consumer.Close();
        }
    }

}