namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class UpdateBalanceDto
    {
        public decimal NewOutstandingBalance { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public string UpdateReason { get; set; } = string.Empty;
        public string? ApprovalReference { get; set; }
        public string? Comments { get; set; }
    }

}
