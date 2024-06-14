using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccessLayer.Concrete;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using EMakler.PROAPI.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure strongly typed settings objects
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Register the database context
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmaklerPRO")));

// Register services and repositories
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

// Kafka configuration
var kafkaBootstrapServers = builder.Configuration["Kafka:BootstrapServers"];
var kafkaTopic = builder.Configuration["Kafka:Topic"];
var kafkaGroupId = builder.Configuration["Kafka:GroupId"];

builder.Services.AddScoped<IProducerKafkaService>(provider =>
    new ProducerKafkaService(kafkaBootstrapServers, kafkaTopic,
        provider.GetRequiredService<ILogger<ProducerKafkaService>>()));

builder.Services.AddScoped<IConsumerKafkaService>(provider =>
    new ConsumerKafkaService(kafkaBootstrapServers, kafkaTopic, kafkaGroupId,
        provider.GetRequiredService<ILogger<ConsumerKafkaService>>()));

// Configure JWT authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

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
