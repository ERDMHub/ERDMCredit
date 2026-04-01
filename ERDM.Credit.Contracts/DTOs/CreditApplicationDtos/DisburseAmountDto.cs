namespace ERDM.Credit.Contracts.DTOs.CreditApplicationDtos
{
    public class DisburseAmountDto
    {
        public decimal Amount { get; set; }
        public DateTime DisbursementDate { get; set; } = DateTime.UtcNow;
        public string DisbursementMethod { get; set; } = string.Empty;
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string? BankCode { get; set; }
        public string? AccountHolderName { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public string DisbursedBy { get; set; } = string.Empty;
        public string? ApprovalReference { get; set; }
        public string? Notes { get; set; }
    }
}
