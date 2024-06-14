namespace BusinessLayer.Interfaces;

public interface IConsumerKafkaService
{
    void Consume(CancellationToken cancellationToken);
}
