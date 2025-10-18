
using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Application.Services.Simulation.Deposit;
using InvestmentSimulator.Application.DTOs;
using InvestmentSimulator.Application.DTOs.Simulation.Deposit;
using Microsoft.OpenApi.Models;
using FluentValidation;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;
using Microsoft.EntityFrameworkCore;
using InvestmentSimulator.API.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);



const string BlazorClientPolicy = "AllowBlazorClient";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar serviços
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(BlazorClientPolicy, policy =>
        policy.WithOrigins("https://localhost:7058", "http://localhost:5097")
              .AllowAnyHeader()
              .AllowAnyMethod());
});


builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<DepositInput>, DepositInputValidator>();
builder.Services.AddScoped<IDepositSimulatorService, DepositSimulatorService>();
builder.Services.AddHttpClient<IEcbDataService, EcbDataService>();

// Adicionar o serviço de cache em memória
builder.Services.AddMemoryCache();
builder.Services.AddAntiforgery();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Investment Simulator API", Version = "v1" });
});

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investment Simulator API v1"));
}

app.UseHttpsRedirection();

app.UseCors(BlazorClientPolicy);
app.UseAntiforgery();

app.MapControllers();

app.Run();