using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EMakler.PROAPI.Entities.Profiles;
using BusinessLayer.Interfaces.AuthService;
using BusinessLayer.Services.AuthService;
using BusinessLayer.Services.JwtService;
using FluentValidation.AspNetCore;
using BusinessLayer.Validators.User;
using BusinessLayer.Configurations;
using BusinessLayer.Services.UserServices;
using BusinessLayer.Interfaces.UserServices;
using BusinessLayer.Services.PostServices;
using BusinessLayer.Interfaces.PostServices;
namespace BusinessLayer;

public static class ServiceRegistration
{
    public static IServiceCollection AddBLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddTransient<JWTService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IPostService, PostService>();

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserRegisterRequestValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserDtoValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UpdateUserDtoValidator>());

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
