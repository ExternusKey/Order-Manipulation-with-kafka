using System.Text.Json;
using Confluent.Kafka;
using ServicesManipulation.Data;
using ServicesManipulation.Models;

namespace ServicesManipulation.Messaging;

public class ClientOrderUpdaterConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const string TopicName = "ConfirmationQueue";
    private readonly IConsumer<string, string> _consumer;

    public ClientOrderUpdaterConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
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

                await SendConfirmedOrderToUser(confirmatedOrder);
                Console.WriteLine($"[Web-Client] Order confirmed: {confirmatedOrder.OrderId} ");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Web-Client] Cancellation requested. Error: {e.Message}");
        }
    }

    private async Task SendConfirmedOrderToUser(OrderConfirmation confirmedOrder)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Console.WriteLine($"[Web-Client] Order confirmed: {confirmedOrder.OrderId}");
        dbContext.Add(confirmedOrder);
        await dbContext.SaveChangesAsync();
    }
}