using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulator.Domain.Models
{
    public class CryptoInvestment
    {
        [Required]
        [RegularExpression("bitcoin", ErrorMessage = "Only bitcoin is supported at the moment.")]
        public string? CryptoId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Initial amount must be positive.")]
        public decimal InitialAmount { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
