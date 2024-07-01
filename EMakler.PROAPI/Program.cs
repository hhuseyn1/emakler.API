using BusinessLayer.Interfaces;
using BusinessLayer.Interfaces.KafkaServices;
using BusinessLayer.Services;
using BusinessLayer.Services.KafkaServices;
using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using EMakler.PROAPI.Configurations;
using EMakler.PROAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure strongly typed settings objects
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Identity"));
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));

// Register the database context
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmaklerPRO")));

// Register services and repositories
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IOtpService, OtpService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

// Kafka configuration
builder.Services.AddScoped<IProducerKafkaService>(provider =>
{
    var kafkaSettings = provider.GetRequiredService<IOptions<KafkaSettings>>().Value;
    return new ProducerKafkaService(kafkaSettings.BrokerUrl, kafkaSettings.TopicName,
        provider.GetRequiredService<ILogger<ProducerKafkaService>>());
});

builder.Services.AddScoped<IConsumerKafkaService>(provider =>
{
    var kafkaSettings = provider.GetRequiredService<IOptions<KafkaSettings>>().Value;
    return new ConsumerKafkaService(kafkaSettings.BrokerUrl, kafkaSettings.TopicName, kafkaSettings.GroupId,
        provider.GetRequiredService<ILogger<ConsumerKafkaService>>());
});

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("Identity").Get<JwtSettings>();

if (string.IsNullOrEmpty(jwtSettings.Secret))
{
    throw new ArgumentNullException("JwtSettings:Secret", "JWT Secret is not configured");
}

var secretBytes = Encoding.UTF8.GetBytes(jwtSettings.Secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
