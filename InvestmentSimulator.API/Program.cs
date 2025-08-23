using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Application.DTOs;
using Microsoft.OpenApi.Models;
using FluentValidation;
using InvestmentSimulator.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços
builder.Services.AddControllers();
builder.Services.AddScoped<ISimulationService, SimulationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<Investment>, InvestmentValidator>();
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

app.MapControllers();

app.Run();