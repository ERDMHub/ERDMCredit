namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class CalculateAmortizationDto
    {
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public DateTime StartDate { get; set; }
        public string InterestType { get; set; } = "Reducing";
        public string RepaymentFrequency { get; set; } = "Monthly";
    }
}
