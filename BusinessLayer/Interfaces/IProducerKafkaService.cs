namespace BusinessLayer.Interfaces;

public interface IProducerKafkaService
{
    Task Produce(string key, string value);
}
