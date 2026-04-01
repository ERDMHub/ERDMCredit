using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountCreditLimitAdjustedEvent : DomainEventBase
    {
        public AccountCreditLimitAdjustedEvent(Account account, decimal oldLimit, decimal newLimit, string adjustedBy, string adjustmentReason)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            OldCreditLimit = oldLimit;
            NewCreditLimit = newLimit;
            AdjustedBy = adjustedBy;
            AdjustmentReason = adjustmentReason;
            CurrentOutstandingBalance = account.OutstandingBalance;
            AvailableCredit = account.AvailableCredit;
            AdjustmentDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public decimal OldCreditLimit { get; }
        public decimal NewCreditLimit { get; }
        public string AdjustedBy { get; }
        public string AdjustmentReason { get; }
        public decimal CurrentOutstandingBalance { get; }
        public decimal AvailableCredit { get; }
        public DateTime AdjustmentDate { get; }
    }
}
