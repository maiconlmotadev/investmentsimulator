using System.ComponentModel.DataAnnotations;

namespace InvestmentSimulator.Client.Models.Simulation.Deposit
{
    public class DepositInput
    {
        [Required(ErrorMessage = "O valor inicial é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor inicial deve ser maior que zero.")]
        public decimal InitialAmount { get; set; }
        [Required(ErrorMessage = "O período é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O período deve ser de pelo menos 1 mês.")]
        public int TimePeriodInMonths { get; set; }
        public InvestmentTerm Term { get; set; }
        public decimal? TaxRate { get; set; } // Optional tax rate in percentage
    }
}
