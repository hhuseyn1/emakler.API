using BusinessLayer.Interfaces.UserServices;
using BusinessLayer.Interfaces;
using BusinessLayer.Services.UserServices;
using BusinessLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using BusinessLayer.Interfaces.KafkaServices;
using BusinessLayer.Services.KafkaServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BusinessLayer.Configurations;
using Microsoft.Extensions.Configuration;
using BusinessLayer.Validators;
using EMakler.PROAPI.Entities.Profiles;

namespace BusinessLayer;

public static class ServiceRegistration
{
    public static IServiceCollection AddBLServices(this IServiceCollection services, IConfiguration builder)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IOtpService, OtpService>();

        services.AddTransient<IBuildingService, BuildingService>();

        services.AddScoped<IProducerKafkaService>(provider =>
        {
            var kafkaSettings = provider.GetRequiredService<IOptions<KafkaSettings>>().Value;
            return new ProducerKafkaService(kafkaSettings.BrokerUrl, kafkaSettings.TopicName,
                provider.GetRequiredService<ILogger<ProducerKafkaService>>());
        });

        services.AddScoped<IConsumerKafkaService>(provider =>
        {
            var kafkaSettings = provider.GetRequiredService<IOptions<KafkaSettings>>().Value;
            return new ConsumerKafkaService(kafkaSettings.BrokerUrl, kafkaSettings.TopicName, kafkaSettings.GroupId,
                provider.GetRequiredService<ILogger<ConsumerKafkaService>>());
        });

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<BuildingFilterValidator>();

        return services;
    }
}
