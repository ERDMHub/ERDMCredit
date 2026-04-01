namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class RestructureAccountDto
    {
        public string RestructuredBy { get; set; } = string.Empty;
        public string RestructuringType { get; set; } = string.Empty; // TermExtension, RateReduction, PaymentHoliday, etc.
        public string RestructuringReason { get; set; } = string.Empty;

        // New Terms
        public int? NewTermMonths { get; set; }
        public decimal? NewInterestRate { get; set; }
        public decimal? NewEmiAmount { get; set; }
        public DateTime? NewNextPaymentDueDate { get; set; }
        public int? PaymentHolidayMonths { get; set; }

        // For partial payments
        public decimal? PrincipalWriteOff { get; set; }
        public decimal? InterestWriteOff { get; set; }

        public string? Comments { get; set; }
    }
}
