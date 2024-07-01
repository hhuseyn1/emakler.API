using BusinessLayer.Interfaces.KafkaServices;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.Services.KafkaServices;

public class ProducerKafkaService : IProducerKafkaService
{
    private readonly string _bootstrapServers;
    private readonly string _topic;
    private readonly ILogger<ProducerKafkaService> _logger;
    public ProducerKafkaService(string bootstrapServers, string topic, ILogger<ProducerKafkaService> logger)
    {
        _bootstrapServers = bootstrapServers;
        _topic = topic;
        _logger = logger;
    }
    public async Task Produce(string key, string value)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        using (var producer = new ProducerBuilder<string, string>(config).Build())
        {
            try
            {
                var result = await producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = value });
                _logger.LogInformation("Message sent to topic {Topic}, partition {Partition}, offset {Offset}", result.Topic, result.Partition, result.Offset);
            }
            catch (ProduceException<string, string> e)
            {
                _logger.LogError("Delivery failed: {Reason}", e.Error.Reason);
            }
        }

    }
}
