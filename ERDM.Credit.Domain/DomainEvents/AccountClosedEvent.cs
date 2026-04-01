using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountClosedEvent : DomainEventBase
    {
        public AccountClosedEvent(Account account, string closedBy, string closureReason, decimal finalBalance)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            ClosedBy = closedBy;
            ClosureReason = closureReason;
            FinalBalance = finalBalance;
            TotalPrincipalPaid = account.PrincipalAmount - account.OutstandingBalance;
            TotalInterestPaid = account.PaymentHistory.Sum(p => p.InterestPaid);
            ClosureDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public string ClosedBy { get; }
        public string ClosureReason { get; }
        public decimal FinalBalance { get; }
        public decimal TotalPrincipalPaid { get; }
        public decimal TotalInterestPaid { get; }
        public DateTime ClosureDate { get; }
    }
}
