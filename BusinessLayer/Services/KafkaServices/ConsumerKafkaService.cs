using BusinessLayer.Interfaces.KafkaServices;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.KafkaServices;

public class ConsumerKafkaService : IConsumerKafkaService
{
    private readonly string _bootstrapServers;
    private readonly string _topic;
    private readonly string _groupId;
    private readonly ILogger<ConsumerKafkaService> _logger;

    public ConsumerKafkaService(string bootstrapServers, string topic, string groupId, ILogger<ConsumerKafkaService> logger)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
        _groupId = groupId;
        _logger = logger;
    }

    public void Consume(CancellationToken cancellationToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = _groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<string, string>(config).Build())
        {
            consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(cancellationToken);
                    _logger.LogInformation("Message: {Message}, Key: {Key}, Topic: {Topic}, Partition: {Partition}, Offset: {Offset}",
                        consumeResult.Message.Value, consumeResult.Message.Key, consumeResult.Topic, consumeResult.Partition, consumeResult.Offset);
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
                _logger.LogInformation("Consumer operation cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming messages.");
            }
        }
    }
}
