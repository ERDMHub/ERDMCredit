using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountDelinquentEvent : DomainEventBase
    {
        public AccountDelinquentEvent(Account account, int daysOverdue, decimal overdueAmount)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            DaysOverdue = daysOverdue;
            OverdueAmount = overdueAmount;
            OutstandingBalance = account.OutstandingBalance;
            DelinquencyBucket = GetDelinquencyBucket(daysOverdue);
            EscalationLevel = DetermineEscalationLevel(daysOverdue);
            DetectedAt = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public int DaysOverdue { get; }
        public decimal OverdueAmount { get; }
        public decimal OutstandingBalance { get; }
        public string DelinquencyBucket { get; }
        public int EscalationLevel { get; }
        public DateTime DetectedAt { get; }

        private static string GetDelinquencyBucket(int daysOverdue)
        {
            return daysOverdue switch
            {
                <= 30 => "1-30 Days",
                <= 60 => "31-60 Days",
                <= 90 => "61-90 Days",
                _ => "90+ Days"
            };
        }

        private static int DetermineEscalationLevel(int daysOverdue)
        {
            return daysOverdue switch
            {
                <= 30 => 1,
                <= 60 => 2,
                <= 90 => 3,
                _ => 4
            };
        }
    }
}
