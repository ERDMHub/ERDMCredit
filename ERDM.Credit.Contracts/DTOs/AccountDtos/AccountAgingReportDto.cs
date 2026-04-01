using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountAgingReportDto
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public AccountStatus Status { get; set; }
        public decimal OutstandingBalance { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDueDate { get; set; }
        public int DaysOverdue { get; set; }
        public string AgingBucket { get; set; } = string.Empty; // 0-30, 31-60, 61-90, 90+
        public decimal OverdueAmount { get; set; }
    }
}
