using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class InvalidRuleConditionEvent : DomainEventBase
    {
        public InvalidRuleConditionEvent(UnderwritingRule rule, string conditionExpression, string error, string detectedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            ConditionExpression = conditionExpression;
            Error = error;
            DetectedBy = detectedBy;
            DetectionDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string ConditionExpression { get; }
        public string Error { get; }
        public string DetectedBy { get; }
        public DateTime DetectionDate { get; }
    }
}
