namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class ApproveCreditDecisionDto
    {
        public string ApprovedBy { get; set; } = string.Empty;
        public decimal? ApprovedAmount { get; set; }
        public decimal? ApprovedInterestRate { get; set; }
        public int? ApprovedTermMonths { get; set; }
        public string? Comments { get; set; }
    }
}
