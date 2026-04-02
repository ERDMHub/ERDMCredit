using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
     // Event raised when a temporary limit expires
    public class TemporaryLimitExpiredEvent : DomainEventBase
    {
        public TemporaryLimitExpiredEvent(LimitHistory limitHistory, DateTime expiryDate)
        {
            EntityId = limitHistory.Id;
            EntityType = nameof(LimitHistory);
            LimitHistoryId = limitHistory.LimitHistoryId;
            CustomerId = limitHistory.CustomerId;
            AccountId = limitHistory.AccountId;
            AccountNumber = limitHistory.AccountNumber;
            ExpiredLimit = limitHistory.NewLimit;
            OriginalLimit = limitHistory.PreviousLimit;
            ExpiryDate = expiryDate;
            WasTemporary = limitHistory.IsTemporary;
        }

        public string LimitHistoryId { get; }
        public string CustomerId { get; }
        public string AccountId { get; }
        public string AccountNumber { get; }
        public decimal ExpiredLimit { get; }
        public decimal OriginalLimit { get; }
        public DateTime ExpiryDate { get; }
        public bool WasTemporary { get; }
    }
}