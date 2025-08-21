using HotelBookingApi.Persistence;
using HotelBookingApi.Services;
using Microsoft.EntityFrameworkCore;

using FluentValidation;
using FluentValidation.AspNetCore;

using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.

services.AddControllers();

//services.AddControllers()
        //.AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter())); // if you use DateOnly

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// EF Core
var cs = builder.Configuration.GetConnectionString("Default") ?? "Data Source=hotel.db";
services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(cs));

// CORS (optional for external testing tools)
services.AddCors(
    opt => opt.AddDefaultPolicy(
                policy =>  policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                                )
                );

// Register the seeder
services.AddScoped<IDataSeeder, DataSeeder>();

// Add FluentValidation
// If your validators live in a different assembly, ... 
// ... swap Program for any type in that assembly.
services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<Program>();

//THE API REQUIRES NO AUTHENTICATION.
// If present, keep your existing auth packages/references.
// Register a permissive policy:
services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAssertion(_ => true) // always pass
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthorization(); // keeps pipeline intact for later

app.UseCors();
app.MapControllers();

app.Run();


/// <summary>
/// You can use WebApplicationFactory<Program> for integration tests, 
/// or test availability logic against an in‑memory SQLite database.
/// </summary>
public partial class Program { } // for WebApplicationFactory