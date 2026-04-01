using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when an underwriting condition is met
    public class UnderwritingConditionMetEvent : DomainEventBase
    {
        public UnderwritingConditionMetEvent(CreditDecision decision, UnderwritingCondition condition, string metBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            ConditionId = condition.ConditionId;
            ConditionType = condition.ConditionType;
            ConditionDescription = condition.Description;
            MetBy = metBy;
            MetDate = condition.MetDate ?? DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string ConditionId { get; }
        public string ConditionType { get; }
        public string ConditionDescription { get; }
        public string MetBy { get; }
        public DateTime MetDate { get; }
    }
}
