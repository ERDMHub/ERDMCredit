using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{

    // Event raised when a decision is referred for manual review
    public class CreditDecisionReferredEvent : DomainEventBase
    {
        public CreditDecisionReferredEvent(CreditDecision decision, string referredBy, string referralReason)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            ReferredBy = referredBy;
            ReferralReason = referralReason;
            ReferralDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string ReferredBy { get; }
        public string ReferralReason { get; }
        public DateTime ReferralDate { get; }
    }

}
