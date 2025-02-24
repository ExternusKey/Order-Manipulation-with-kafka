using System.Text.Json;
using Confluent.Kafka;
using DbClientService.Controllers;
using DbClientService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DbClientService.Messaging;

public class ServerOrderConsumer : BackgroundService
{
    private const string TopicName = "OrderQueue";
    private readonly IConsumer<string, string> _consumer;
    private readonly NewOrderController _newOrderController;
    public ServerOrderConsumer(NewOrderController newOrderController)
    {
        _newOrderController = newOrderController;
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
                    
                    var response = await _newOrderController.PostNewOrderData(order);
                    if (response is ObjectResult { StatusCode: 500 } objectResult)
                    {
                        var errorDetails = objectResult.Value as dynamic;
                        var errorMessage = errorDetails?.message;
                        var errorDetailsMessage = errorDetails?.details;
                        
                        Console.WriteLine($"[Back-Client] Error: {errorMessage}, Details: {errorDetailsMessage}");
                    }
                    var confirmedOrder = new OrderConfirmation
                    {
                        OrderId = order.OrderId,
                        ProductId = order.ProductId,
                        GpuName = order.GpuName,
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
            Console.WriteLine($"[Back-Client] Cancellation requested. Error: {e.Message}");
        }
        finally
        {
            _consumer.Close();
        }
    }
}