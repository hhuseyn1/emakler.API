namespace BusinessLayer.Configurations;

public class KafkaSettings
{
        public string BrokerUrl { get; set; }
        public string TopicName { get; set; }
        public string GroupId { get; set; }
}
