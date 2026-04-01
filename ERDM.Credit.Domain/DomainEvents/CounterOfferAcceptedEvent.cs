using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a counter offer is accepted
    public class CounterOfferAcceptedEvent : DomainEventBase
    {
        public CounterOfferAcceptedEvent(CreditDecision decision, string acceptedBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            AcceptedBy = acceptedBy;
            AcceptedAmount = decision.CounterOfferAmount;
            AcceptedInterestRate = decision.CounterOfferInterestRate;
            AcceptedTermMonths = decision.CounterOfferTermMonths;
            AcceptanceDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string AcceptedBy { get; }
        public decimal? AcceptedAmount { get; }
        public decimal? AcceptedInterestRate { get; }
        public int? AcceptedTermMonths { get; }
        public DateTime AcceptanceDate { get; }
    }
}
