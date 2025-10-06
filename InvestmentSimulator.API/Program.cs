
using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Application.Services.Simulation.Deposit;
using InvestmentSimulator.Application.DTOs;
using InvestmentSimulator.Application.DTOs.Simulation.Deposit;
using Microsoft.OpenApi.Models;
using FluentValidation;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;

var builder = WebApplication.CreateBuilder(args);



var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Adicionar serviços
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5097", "https://localhost:7058") // Adicionada a porta do cliente Blazor
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});

var app = builder.Build();

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investment Simulator API v1"));
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
app.UseAntiforgery();

app.MapControllers();

app.Run();