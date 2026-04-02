using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a limit change is reverted
    public class LimitHistoryRevertedEvent : DomainEventBase
    {
        public LimitHistoryRevertedEvent(LimitHistory limitHistory, decimal previousLimit, decimal newLimit, string revertedBy, string revertReason)
        {
            EntityId = limitHistory.Id;
            EntityType = nameof(LimitHistory);
            LimitHistoryId = limitHistory.LimitHistoryId;
            CustomerId = limitHistory.CustomerId;
            AccountId = limitHistory.AccountId;
            AccountNumber = limitHistory.AccountNumber;
            PreviousLimit = previousLimit;
            NewLimit = newLimit;
            RevertedBy = revertedBy;
            RevertReason = revertReason;
            RevertDate = DateTime.UtcNow;
        }

        public string LimitHistoryId { get; }
        public string CustomerId { get; }
        public string AccountId { get; }
        public string AccountNumber { get; }
        public decimal PreviousLimit { get; }
        public decimal NewLimit { get; }
        public string RevertedBy { get; }
        public string RevertReason { get; }
        public DateTime RevertDate { get; }
    }
}