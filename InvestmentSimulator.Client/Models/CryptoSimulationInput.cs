namespace InvestmentSimulator.Client.Models
{
    public class CryptoSimulationInput
    {
        public string? CryptoId { get; set; }
        public decimal InitialAmount { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}