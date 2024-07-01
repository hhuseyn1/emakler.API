namespace BusinessLayer.Interfaces.KafkaServices;

public interface IProducerKafkaService
{
    Task ProduceAsync(string key, string value);
}
