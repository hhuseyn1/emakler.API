namespace BusinessLayer.Interfaces.KafkaServices;

public interface IProducerKafkaService
{
    Task Produce(string key, string value);
}
