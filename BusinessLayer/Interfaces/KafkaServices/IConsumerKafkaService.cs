namespace BusinessLayer.Interfaces.KafkaServices;

public interface IConsumerKafkaService
{
    void Consume(CancellationToken cancellationToken);
}
