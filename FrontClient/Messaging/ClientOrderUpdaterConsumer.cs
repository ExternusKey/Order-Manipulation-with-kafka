using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using ServicesManipulation.Controllers;
using ServicesManipulation.Models;

namespace ServicesManipulation.Messaging;

public class ClientOrderUpdaterConsumer : BackgroundService
{
    private const string TopicName = "ConfirmationQueue";
    private readonly IConsumer<string, string> _consumer;
    private readonly ConfirmationController _confirmationController;

    public ClientOrderUpdaterConsumer(ConfirmationController confirmationController)
    {
        _confirmationController = confirmationController;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            GroupId = "order-updater-consumer",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(TopicName);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = await Task.Run(() => _consumer.Consume(cancellationToken), cancellationToken);
                var confirmatedOrder = JsonSerializer.Deserialize<OrderConfirmation>(consumeResult.Message.Value);
                if (confirmatedOrder == null)
                    continue;
                
                var result = await _confirmationController.SendOrderConfirmationToDbAsync(confirmatedOrder);
                if (result is ObjectResult objectResult && objectResult.StatusCode == 500)
                {

                    var errorDetails = objectResult.Value as dynamic;
                    var errorMessage = errorDetails?.message;
                    var errorDetailsMessage = errorDetails?.details;
                    
                    Console.WriteLine($"[Web-Client] Error: {errorMessage}, Details: {errorDetailsMessage}");
                }
                else
                    Console.WriteLine($"[Web-Client] Order confirmed: {confirmatedOrder.OrderId} ");

            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Web-Client] Cancellation requested. Error: {e.Message}");
        }
    }


}