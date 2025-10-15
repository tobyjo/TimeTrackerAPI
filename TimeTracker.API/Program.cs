using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TimeTracker.API.DbContexts;
using TimeTracker.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Auth0
var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
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
// builder.Services.AddDbContext<TimeEntryContext>(dbContextOptions => dbContextOptions.UseSqlServer(@"Server=localhost\SQLEXPRESS01;Database=timetracker;Trusted_Connection=True;TrustServerCertificate=True;"));

//builder.Services.AddDbContext<TimeEntryContext>(dbContextOptions => dbContextOptions.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=timetracker;Trusted_Connection=True;"));

builder.Services.AddScoped<ITimeTrackerRepository, TimeTrackerRepository>();

// Now need a license (free for now) for AutoMapper - see KeePass
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg5Nzc2MDAwIiwiaWF0IjoiMTc1ODI5NDA2OCIsImFjY291bnRfaWQiOiIwMTk5NjI3YzUwNzA3MTE5YmI1YTFiNDI4MDA1YTQ0MiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazVoN3drODBuOHk1dnM4M2duZXRhdGEyIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.bYu3zZgOgZU_MB4m6HuHm43VTMJZ7VI22hFQUIjpdLaqSFk6dqkXFhkVQ0Ed9mk91kJsk66n5KApZRuTu6OdXje-mlescFwjoBgkzn93Y64Hqsmk2sSZtmVA6N-lVhbgSSr-b2UKcNAeMpr7KwKKFMbJblCobqZHEQ12modaHC4jTaRfv_ZKRQE1oG1dDGvKfH9CoktZ_QJ6PNNES44wgUfFgBwpZBe_pr0K7G80aq8jhfaOn6Iif2vFFzdhr4aAFj0cuShgrkt6sYoNl_exlXzX9-FzNYVsTuvZzHKWfKCxo4HYNnk7jah63ww2JVo9CCxHhbmRBBJJeCKTjAzkTw", AppDomain.CurrentDomain.GetAssemblies());
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