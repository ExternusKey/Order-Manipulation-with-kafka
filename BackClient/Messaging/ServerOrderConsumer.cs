using System.Text.Json;
using Confluent.Kafka;
using DbClientService.Data;
using DbClientService.Models;

namespace DbClientService.Messaging;

public class ServerOrderConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const string TopicName = "OrderQueue";
    private readonly IConsumer<string, string> _consumer;

    public ServerOrderConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "order-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(TopicName);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var serverConfirmationProducer = new ServerOrderConfirmationProducer();
        
        try
        {            
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = await Task.Run(() => _consumer.Consume(cancellationToken), cancellationToken);

                var order = JsonSerializer.Deserialize<OrderRequest>(consumeResult.Message.Value);
                
                Console.WriteLine($"[Back-Client] Order received: {order?.OrderId} for {order?.UserName}");
                if (order != null)
                {
                    await SendOrderToDatabase(order);

                    var confirmedOrder = new OrderConfirmation
                    {
                        OrderId = order.OrderId,
                        ProductId = order.ProductId,
                        UserName = order.UserName,
                        ProcessedBy = "Back-Client",
                        ConfirmedAt = order.OrderDate,
                    };

                    await serverConfirmationProducer.SendOrderConfirmationAsync(confirmedOrder);
                }
            }
        }
        catch (OperationCanceledException e )
        {
            // TODO: LOGGER
            Console.WriteLine($"[Back-Client] Cancellation requested. Error: {e.Message}");
        }
        finally
        {
            _consumer.Close();
        }
    }

    private async Task SendOrderToDatabase(OrderRequest order)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.OrderRequests.Add(order);
        await dbContext.SaveChangesAsync();
    }

}