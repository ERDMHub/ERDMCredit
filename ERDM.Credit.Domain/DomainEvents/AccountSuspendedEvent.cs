using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountSuspendedEvent : DomainEventBase
    {
        public AccountSuspendedEvent(Account account, string suspendedBy, string suspensionReason)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            SuspendedBy = suspendedBy;
            SuspensionReason = suspensionReason;
            OutstandingBalanceAtSuspension = account.OutstandingBalance;
            SuspensionDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public string SuspendedBy { get; }
        public string SuspensionReason { get; }
        public decimal OutstandingBalanceAtSuspension { get; }
        public DateTime SuspensionDate { get; }
    }
}
