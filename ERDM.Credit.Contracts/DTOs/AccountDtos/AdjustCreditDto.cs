namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AdjustCreditDto
    {
        public decimal NewAvailableCredit { get; set; }
        public decimal NewCreditLimit { get; set; }
        public string AdjustedBy { get; set; } = string.Empty;
        public string AdjustmentReason { get; set; } = string.Empty;
        public string? ApprovalReference { get; set; }
        public string? Comments { get; set; }
    }

}
