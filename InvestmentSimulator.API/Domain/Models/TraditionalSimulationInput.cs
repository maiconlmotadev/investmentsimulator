namespace InvestmentSimulator.Domain.Models
{
    public class TraditionalSimulationInput
    {
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public decimal InitialAmount { get; private set; }
        public decimal InterestRate { get; private set; } // Em porcentagem
        public int TimePeriod { get; private set; } // Em meses
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public TraditionalSimulationInput(decimal initialAmount, decimal interestRate, int timePeriod, string userId)
        {
            InitialAmount = initialAmount;
            InterestRate = interestRate;
            TimePeriod = timePeriod;
            UserId = userId;
        }
    }
}
