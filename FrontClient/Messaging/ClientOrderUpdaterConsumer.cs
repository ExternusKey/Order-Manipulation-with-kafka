using System.Text;
using System.Text.Json;
using Confluent.Kafka;
using ServicesManipulation.Models;

namespace ServicesManipulation.Messaging;

public class ClientOrderUpdaterConsumer : BackgroundService
{
    private const string TopicName = "ConfirmationQueue";
    private readonly IConsumer<string, string> _consumer;
    private readonly IHttpClientFactory _httpClientFactory;

    public ClientOrderUpdaterConsumer(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
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
                
                using var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(confirmatedOrder), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:44301/api/Confirmation", content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync(cancellationToken);
                    Console.WriteLine($"[Web-Client] Error: {response.StatusCode}, Details: {errorDetails}");
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
