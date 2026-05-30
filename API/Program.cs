using API.Middlewares;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Progression;
using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Email;
using Infrastructure.Services.Progression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Domain.Interfaces.User;
using Domain.Interfaces.Task;
using Domain.Interfaces.Stats;
using Domain.Interfaces.Journal;
using Domain.Interfaces.Notification;
using Application.Common.Interfaces.Stats;
using MediatR;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LevelUpDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Get JWT secret from environment variable or user-secrets (never from config files)
var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("JWT_SECRET_KEY environment variable or user-secret not configured. Set with: dotnet user-secrets set 'Jwt:SecretKey' 'your-secret-key' (min 32 chars)");

// Validate secret key length (minimum 256 bits = 32 characters)
if (secretKey.Length < 32)
{
    throw new InvalidOperationException("JWT_SECRET_KEY must be at least 32 characters long for security");
}

var signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Register Application Services
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Application.Features.Auth.Commands.RegisterCommand).Assembly);
});

// Register Infrastructure Services
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAvatarService, AvatarService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Register Progression Services
builder.Services.AddScoped<IXPService, XPService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IHPService, HPService>();
builder.Services.AddScoped<IStreakService, StreakService>();
builder.Services.AddScoped<IShieldService, ShieldService>();

// Register Stats Services
builder.Services.AddScoped<IStatsRepository, StatsRepository>();
builder.Services.AddScoped<IStatsService, Infrastructure.Services.Stats.StatsService>();

// Register Journal Services
builder.Services.AddScoped<IJournalRepository, JournalRepository>();

// Register Notification Services
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IReminderService, ReminderService>();

builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Add Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    // Stricter limits for auth endpoints (prevent brute force) - 5 requests per minute per IP
    options.AddFixedWindowLimiter("auth", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
    });

    // Global rate limit - 100 requests per minute
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }
        )
    );

    // Return 429 Too Many Requests when rate limit exceeded
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// Add Swagger/Swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use middlewares
app.UseRateLimiter();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "LevelUp API v1");
        options.RoutePrefix = string.Empty; // Serve Swagger UI at the root
    });
}
else
{
    // Only redirect to HTTPS in production
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
