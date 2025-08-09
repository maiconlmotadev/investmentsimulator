using InvestmentSimulator.Application.DTOs;
using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Domain.Models;
using System.Security.Cryptography;
using System.Text;

namespace InvestmentSimulator.Application.Services
{
    public class UserService : IUserService
    {
        // No futuro, você injetaria aqui seu DbContext ou Repositório
        // private readonly IUserRepository _userRepository;

        public UserService(/*IUserRepository userRepository*/)
        {
            // _userRepository = userRepository;
        }

        public Task<User> RegisterAsync(UserDto userDto)
        {
            // 1. Hash da senha (lógica de aplicação)
            var passwordHash = HashPassword(userDto.Password);

            // 2. Criação do modelo de domínio
            var user = new User(userDto.Username, userDto.Email, passwordHash);

            // 3. Salvar no banco de dados (a ser implementado)
            // await _userRepository.AddAsync(user);

            // Por enquanto, apenas retornamos o usuário criado
            return Task.FromResult(user);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
