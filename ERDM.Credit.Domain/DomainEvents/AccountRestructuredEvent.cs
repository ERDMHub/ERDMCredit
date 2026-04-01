using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountRestructuredEvent : DomainEventBase
    {
        public AccountRestructuredEvent(Account account, RestructuringDetails restructuringDetails)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            OldOutstandingBalance = restructuringDetails.OldOutstandingBalance;
            NewOutstandingBalance = account.OutstandingBalance;
            OldInterestRate = restructuringDetails.OldInterestRate;
            NewInterestRate = account.InterestRate;
            OldTermMonths = restructuringDetails.OldTermMonths;
            NewTermMonths = account.TermMonths;
            OldEMI = restructuringDetails.OldEMI;
            NewEMI = account.EmiAmount;
            RestructuringType = restructuringDetails.RestructuringType;
            RestructuringReason = restructuringDetails.RestructuringReason;
            RestructuredBy = restructuringDetails.RestructuredBy;
            RestructuringDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public decimal OldOutstandingBalance { get; }
        public decimal NewOutstandingBalance { get; }
        public decimal OldInterestRate { get; }
        public decimal NewInterestRate { get; }
        public int OldTermMonths { get; }
        public int NewTermMonths { get; }
        public decimal OldEMI { get; }
        public decimal NewEMI { get; }
        public string RestructuringType { get; }
        public string RestructuringReason { get; }
        public string RestructuredBy { get; }
        public DateTime RestructuringDate { get; }
    }

}
