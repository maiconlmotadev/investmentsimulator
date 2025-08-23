namespace InvestmentSimulator.Domain.Models
{
    public class User
    {
        public int Id { get; private set; }
        public string? Username { get; private set; }
        public string? Email { get; private set; }
        public string? PasswordHash { get; private set; }

        // Construtor para criar um novo usuário
        public User(string username, string email, string passwordHash)
        {
            // Validações podem ser adicionadas aqui no futuro, se necessário
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }

        // Construtor vazio para o Entity Framework (se for usar)
        private User() { }
    }
}