namespace InvestmentSimulator.Domain.Models
{
    public class Investment
    {
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public decimal InitialAmount { get; private set; }
        public decimal InterestRate { get; private set; } // Em porcentagem
        public int TimePeriod { get; private set; } // Em meses
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public Investment(decimal initialAmount, decimal interestRate, int timePeriod, string userId)
        {
            if (initialAmount <= 0) throw new ArgumentException("Initial amount must be positive.");
            if (interestRate < 0) throw new ArgumentException("Interest rate cannot be negative.");
            if (timePeriod <= 0) throw new ArgumentException("Time period must be positive.");

            InitialAmount = initialAmount;
            InterestRate = interestRate;
            TimePeriod = timePeriod;
            UserId = userId;
        }
    }
}