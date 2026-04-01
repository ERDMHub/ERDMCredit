using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class LateFeeAppliedEvent : DomainEventBase
    {
        public LateFeeAppliedEvent(Account account, decimal lateFeeAmount, int daysOverdue, string appliedBy)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            LateFeeAmount = lateFeeAmount;
            DaysOverdue = daysOverdue;
            AppliedBy = appliedBy;
            OutstandingBalanceAfterFee = account.OutstandingBalance;
            AppliedDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public decimal LateFeeAmount { get; }
        public int DaysOverdue { get; }
        public string AppliedBy { get; }
        public decimal OutstandingBalanceAfterFee { get; }
        public DateTime AppliedDate { get; }
    }
}
