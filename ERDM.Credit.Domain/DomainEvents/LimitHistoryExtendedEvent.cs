using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a limit history is extended
    public class LimitHistoryExtendedEvent : DomainEventBase
    {
        public LimitHistoryExtendedEvent(LimitHistory limitHistory, DateTime newExpiryDate, string extendedBy)
        {
            EntityId = limitHistory.Id;
            EntityType = nameof(LimitHistory);
            LimitHistoryId = limitHistory.LimitHistoryId;
            CustomerId = limitHistory.CustomerId;
            AccountId = limitHistory.AccountId;
            AccountNumber = limitHistory.AccountNumber;
            PreviousExpiryDate = limitHistory.ExpiryDate;
            NewExpiryDate = newExpiryDate;
            ExtendedBy = extendedBy;
            ExtensionDate = DateTime.UtcNow;
        }

        public string LimitHistoryId { get; }
        public string CustomerId { get; }
        public string AccountId { get; }
        public string AccountNumber { get; }
        public DateTime? PreviousExpiryDate { get; }
        public DateTime NewExpiryDate { get; }
        public string ExtendedBy { get; }
        public DateTime ExtensionDate { get; }
    }

    
}