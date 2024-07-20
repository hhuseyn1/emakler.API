using DataAccessLayer.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer.Repository;

namespace DataAccessLayer;

public static class ServiceRegistration
{
    public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
          options.UseSqlServer(configuration.GetConnectionString("EmaklerPRO")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}
