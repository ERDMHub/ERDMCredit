namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AgingAccountDto
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = string.Empty;
        public decimal OutstandingBalance { get; set; }
        public decimal OverdueAmount { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDueDate { get; set; }
        public int DaysOverdue { get; set; }
        public string AgingBucket { get; set; } = string.Empty;
        public string? AssignedOfficer { get; set; }
        public string? BranchCode { get; set; }
        public int CollectionAttempts { get; set; }
        public DateTime? LastCollectionAttempt { get; set; }
    }
}
