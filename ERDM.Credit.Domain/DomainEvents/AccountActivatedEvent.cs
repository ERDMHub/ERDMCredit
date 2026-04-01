using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountActivatedEvent : DomainEventBase
    {
        public AccountActivatedEvent(Account account, string activatedBy, string activationReason)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            ApplicationId = account.ApplicationId;
            ActivatedBy = activatedBy;
            ActivationReason = activationReason;
            ActivatedAmount = account.DisbursedAmount;
            ActivationDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public string ApplicationId { get; }
        public string ActivatedBy { get; }
        public string ActivationReason { get; }
        public decimal ActivatedAmount { get; }
        public DateTime ActivationDate { get; }
    }
}
