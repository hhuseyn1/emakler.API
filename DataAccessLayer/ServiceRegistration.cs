using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class ServiceRegistration
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
          options.UseSqlServer(configuration.GetConnectionString("EmaklerPRO")));

        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IBuildingRepository, BuildingRepository>();

        return services;
    }
}
