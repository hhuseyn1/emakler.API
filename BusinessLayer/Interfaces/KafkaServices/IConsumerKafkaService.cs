namespace BusinessLayer.Interfaces.KafkaServices;

public interface IConsumerKafkaService
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}
