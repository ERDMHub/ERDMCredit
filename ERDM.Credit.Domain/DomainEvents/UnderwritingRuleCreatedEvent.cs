using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleCreatedEvent : DomainEventBase
    {
        public UnderwritingRuleCreatedEvent(UnderwritingRule rule)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            RuleType = rule.RuleType;
            Category = rule.Category;
            Priority = rule.Priority;
            Condition = rule.Condition;
            CreatedBy = rule.CreatedBy;
            CreatedAt = rule.CreatedAt.Value;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public RuleType RuleType { get; }
        public RuleCategory Category { get; }
        public int Priority { get; }
        public string Condition { get; }
        public string CreatedBy { get; }
        public DateTime CreatedAt { get; }
    }

}
