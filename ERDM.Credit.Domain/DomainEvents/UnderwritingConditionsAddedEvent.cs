using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when underwriting conditions are added
    public class UnderwritingConditionsAddedEvent : DomainEventBase
    {
        public UnderwritingConditionsAddedEvent(CreditDecision decision, List<UnderwritingCondition> conditions, string addedBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            Conditions = conditions;
            AddedBy = addedBy;
            AddedDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public List<UnderwritingCondition> Conditions { get; }
        public string AddedBy { get; }
        public DateTime AddedDate { get; }
    }
}
