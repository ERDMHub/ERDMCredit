using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class CreditDecisionCreatedEvent : DomainEventBase
    {
        public CreditDecisionCreatedEvent(CreditDecision decision)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            DecisionType = decision.DecisionType;
            DecisionBy = decision.DecisionBy;
            DecisionDate = decision.DecisionDate;
            IsCounterOffer = decision.IsCounterOffer;
            ApprovedAmount = decision.ApprovedAmount;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public DecisionType DecisionType { get; }
        public string DecisionBy { get; }
        public DateTime DecisionDate { get; }
        public bool IsCounterOffer { get; }
        public decimal? ApprovedAmount { get; }
    }
}
