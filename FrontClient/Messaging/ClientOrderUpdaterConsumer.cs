using System.Text.Json;
using Confluent.Kafka;
using ServicesManipulation.Models;

namespace ServicesManipulation.Messaging;

public class ClientOrderUpdaterConsumer
{
    private const string TopicName = "ConfirmationQueue";
    private readonly IConsumer<string, string> _consumer;

    ClientOrderUpdaterConsumer()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "order-updater-consumer",
            AutoOffsetReset = AutoOffsetReset.Earliest,
        };
        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(TopicName);
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                var confirmation = JsonSerializer.Deserialize<Confirmation>(consumeResult.Message.Value);
                
                if (confirmation == null)
                    continue;
                
                Console.WriteLine($"[Web-Client] Order confirmed: {confirmation.OrderId} ");
                
                // TODO: Добавление записи в таблицу User-orders-list
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