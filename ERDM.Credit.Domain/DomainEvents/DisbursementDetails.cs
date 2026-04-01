namespace ERDM.Credit.Domain.DomainEvents
{
    public class DisbursementDetails
    {
        public decimal Amount { get; set; }
        public DateTime DisbursementDate { get; set; }
        public string DisbursementMethod { get; set; } = string.Empty;
        public string? BankAccountNumber { get; set; }
        public string? BankName { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public string DisbursedBy { get; set; } = string.Empty;
    }
}
