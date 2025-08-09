using InvestmentSimulator.Application.DTOs;
using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserDto userDto);
    }
}
