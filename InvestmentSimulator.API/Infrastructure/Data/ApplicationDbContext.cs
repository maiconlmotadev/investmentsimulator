
using InvestmentSimulator.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestmentSimulator.API.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
