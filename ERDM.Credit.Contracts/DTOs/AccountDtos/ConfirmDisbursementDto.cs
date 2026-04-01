namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class ConfirmDisbursementDto
    {
        public string ConfirmedBy { get; set; } = string.Empty;
        public DateTime ConfirmationDate { get; set; } = DateTime.UtcNow;
        public string TransactionReference { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; } = true;
        public string? FailureReason { get; set; }
        public string? Notes { get; set; }
    }
}
