using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountSummaryDto
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int TotalAccounts { get; set; }
        public int ActiveAccounts { get; set; }
        public int ClosedAccounts { get; set; }

        public decimal TotalOutstandingBalance { get; set; }
        public decimal TotalAvailableCredit { get; set; }
        public decimal TotalDisbursedAmount { get; set; }

        public decimal TotalMonthlyObligation { get; set; }
        public decimal TotalPrincipalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }

        public List<AccountSummaryItemDto> Accounts { get; set; } = new();
        public PaymentBehaviorDto PaymentBehavior { get; set; } = new();
    }
}
