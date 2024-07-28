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
using BusinessLayer.Interfaces.OtpService;
using BusinessLayer.Exception;
using BusinessLayer.Validators.Post;


namespace BusinessLayer;

public static class ServiceRegistration
{
    public static IServiceCollection AddBLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddTransient<JWTService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IOtpService, OtpService>();

        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateBuildingPostDtoValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserRegisterRequestValidator>());
        services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserDtoValidator>());

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}
