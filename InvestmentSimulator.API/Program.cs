using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Application.Services.Simulation.Deposit;
using InvestmentSimulator.Application.DTOs;
using InvestmentSimulator.Application.DTOs.Simulation.Deposit;
using Microsoft.OpenApi.Models;
using FluentValidation;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços
builder.Services.AddControllers();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<DepositInput>, DepositInputValidator>();
builder.Services.AddScoped<IDepositSimulatorService, DepositSimulatorService>();
builder.Services.AddHttpClient<IEcbDataService, EcbDataService>();

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

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();