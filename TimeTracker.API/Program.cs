using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TimeTracker.API.DbContexts;
using TimeTracker.API.Services;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add file logging provider explicitly for Azure
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

Console.WriteLine("=== STARTUP: Building application ===");

// Explicitly load appsettings.Local.json if it exists
var localSettingsPath = Path.Combine(builder.Environment.ContentRootPath, "appsettings.Local.json");
if (File.Exists(localSettingsPath))
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
    Console.WriteLine("STARTUP: Loaded appsettings.Local.json");
}

// Configure Azure Key Vault integration
var keyVaultName = builder.Configuration["Azure:KeyVaultName"];
Console.WriteLine($"STARTUP: Key Vault Name from config: '{keyVaultName}'");

if (!string.IsNullOrEmpty(keyVaultName))
{
    try
    {
        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        Console.WriteLine($"STARTUP: Attempting to connect to Key Vault: {keyVaultUri}");
        builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
        Console.WriteLine("STARTUP: Successfully connected to Key Vault!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"STARTUP ERROR: Failed to connect to Key Vault: {ex.Message}");
        Console.WriteLine($"STARTUP ERROR: Stack trace: {ex.StackTrace}");
    }
}
else
{
    Console.WriteLine("STARTUP: No Key Vault configured");
}

// Add Auth0
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
var audience = builder.Configuration["Auth0:Audience"];

Console.WriteLine($"STARTUP: Auth0 Domain: {builder.Configuration["Auth0:Domain"]}");
Console.WriteLine($"STARTUP: Auth0 Audience: {audience}");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// Register as singleton
builder.Services.AddSingleton<TimeTrackerDataStore>();

// Register in-memory logger for F1 tier
builder.Services.AddSingleton<IInMemoryLogger, InMemoryLogger>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TimeTrackerContext>(dbContextOptions => dbContextOptions.UseSqlServer(builder.Configuration.GetConnectionString("TimeTrackerDBConnectionString")));

builder.Services.AddScoped<ITimeTrackerRepository, TimeTrackerRepository>();

// AutoMapper license key now loaded from configuration
var autoMapperLicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapperLicenseKey, AppDomain.CurrentDomain.GetAssemblies());

Console.WriteLine("STARTUP: Building app...");
var app = builder.Build();
Console.WriteLine("STARTUP: App built successfully!");

// Get the in-memory logger
var inMemoryLogger = app.Services.GetRequiredService<IInMemoryLogger>();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== Application Starting ===");
logger.LogInformation("Environment: {EnvironmentName}", app.Environment.EnvironmentName);
logger.LogInformation("Key Vault Name: {KeyVaultName}", keyVaultName ?? "(not configured)");
logger.LogInformation("Auth0 Domain: {Domain}", builder.Configuration["Auth0:Domain"]);

// Also log to in-memory logger
inMemoryLogger.Log("Application starting");
inMemoryLogger.Log($"Environment: {app.Environment.EnvironmentName}");
inMemoryLogger.Log($"Key Vault: {keyVaultName ?? "not configured"}");
inMemoryLogger.Log($"Auth0 Domain: {builder.Configuration["Auth0:Domain"]}");

Console.WriteLine($"STARTUP: Environment: {app.Environment.EnvironmentName}");

// Add request logging middleware
app.Use(async (context, next) =>
{
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    var logMessage = $"{context.Request.Method} {context.Request.Path}";
    Console.WriteLine($"{timestamp} - {logMessage}");
    inMemoryLogger.Log(logMessage);
    
    await next();
    
    logMessage = $"{context.Request.Method} {context.Request.Path} - {context.Response.StatusCode}";
    Console.WriteLine($"{timestamp} - {logMessage}");
    inMemoryLogger.Log($"{logMessage}");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var mapper = app.Services.GetRequiredService<IMapper>();
    mapper.ConfigurationProvider.AssertConfigurationIsValid();

    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    // For React running locally
    app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:5173", "http://localhost:5174", "http://192.168.1.13:5173", "http://192.168.1.14:5173")
        .AllowAnyMethod()
        .AllowAnyHeader());
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", (ILogger<Program> healthLogger, IConfiguration config, IInMemoryLogger memLogger) =>
{
    Console.WriteLine("HEALTH: Health check endpoint called!");
    healthLogger.LogInformation("Health check endpoint called!");
    memLogger.Log("Health check endpoint called");
 
    return Results.Ok(new
  {
      status = "Healthy",
        timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName,
        keyVaultConfigured = !string.IsNullOrEmpty(keyVaultName),
    diagnostics = new
  {
keyVaultName = keyVaultName,
       auth0Domain = config["Auth0:Domain"],
      auth0Audience = config["Auth0:Audience"],
        hasConnectionString = !string.IsNullOrEmpty(config.GetConnectionString("TimeTrackerDBConnectionString")),
      hasAutoMapperKey = !string.IsNullOrEmpty(config["AutoMapper:LicenseKey"]),
     connectionStringLength = config.GetConnectionString("TimeTrackerDBConnectionString")?.Length ?? 0
   }
    });
});

logger.LogInformation("Application configured and ready to handle requests");
Console.WriteLine("STARTUP: Application configured and ready!");
inMemoryLogger.Log("Application configured and ready to handle requests");

// Test endpoint without authentication (for debugging)
app.MapGet("/api/test/ping", (IInMemoryLogger memLogger) =>
{
    memLogger.Log("Ping endpoint called");
    return Results.Ok(new
    {
        message = "Pong!",
        timestamp = DateTime.UtcNow,
        environment = app.Environment.EnvironmentName
    });
});

// Custom in-memory log endpoint for F1 tier
app.MapGet("/debug/logs", (IInMemoryLogger memLogger) =>
{
    var debugInfo = new
    {
     timestamp = DateTime.UtcNow,
        logs = memLogger.GetRecentLogs(50),
        systemInfo = new
        {
   environment = app.Environment.EnvironmentName,
            contentRootPath = app.Environment.ContentRootPath,
          keyVaultName = keyVaultName,
     processId = Environment.ProcessId,
          machineName = Environment.MachineName,
       osVersion = Environment.OSVersion.ToString(),
            dotnetVersion = Environment.Version.ToString()
     }
    };
    
    return Results.Ok(debugInfo);
});

app.Run();