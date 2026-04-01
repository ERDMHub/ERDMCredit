namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountSummaryItemDto
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = string.Empty;
        public decimal OutstandingBalance { get; set; }
        public decimal EmiAmount { get; set; }
        public DateTime? NextPaymentDueDate { get; set; }
        public bool IsDelinquent { get; set; }
        public int DaysOverdue { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? MaturityDate { get; set; }
    }
}
