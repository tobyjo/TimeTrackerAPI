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

// Explicitly load appsettings.Local.json if it exists
var localSettingsPath = Path.Combine(builder.Environment.ContentRootPath, "appsettings.Local.json");
if (File.Exists(localSettingsPath))
{
    builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
}

// Configure Azure Key Vault integration
// This uses DefaultAzureCredential which works both locally (via Azure CLI/Visual Studio) and in Azure (via Managed Identity)
var keyVaultName = builder.Configuration["Azure:KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    try
    {
        var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());
    }
    catch (Exception ex)
    {
        // Key Vault connection failed - will fall back to local configuration
        // To use Key Vault locally, run: az login
    }
}

// Add Auth0
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
var audience = builder.Configuration["Auth0:Audience"];

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

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TimeTrackerContext>(dbContextOptions => dbContextOptions.UseSqlServer(builder.Configuration.GetConnectionString("TimeTrackerDBConnectionString")));

builder.Services.AddScoped<ITimeTrackerRepository, TimeTrackerRepository>();

// AutoMapper license key now loaded from configuration (can be stored in Key Vault)
var autoMapperLicenseKey = builder.Configuration["AutoMapper:LicenseKey"];
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = autoMapperLicenseKey, AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();



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

app.Run();