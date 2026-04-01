using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a counter offer expires
    public class CounterOfferExpiredEvent : DomainEventBase
    {
        public CounterOfferExpiredEvent(CreditDecision decision)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            ExpiredAmount = decision.CounterOfferAmount;
            ExpiryDate = decision.CounterOfferExpiryDate ?? DateTime.UtcNow;
            ExpirationDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public decimal? ExpiredAmount { get; }
        public DateTime ExpiryDate { get; }
        public DateTime ExpirationDate { get; }
    }
}
