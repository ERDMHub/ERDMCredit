namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class ReversePaymentDto
    {
        public string ReversedBy { get; set; } = string.Empty;
        public string ReversalReason { get; set; } = string.Empty;
        public DateTime ReversalDate { get; set; } = DateTime.UtcNow;
        public string? ApprovalReference { get; set; }
        public string? Comments { get; set; }
    }

}
