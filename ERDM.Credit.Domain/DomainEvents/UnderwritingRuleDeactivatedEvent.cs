using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleDeactivatedEvent : DomainEventBase
    {
        public UnderwritingRuleDeactivatedEvent(UnderwritingRule rule, string deactivatedBy, string reason)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            DeactivatedBy = deactivatedBy;
            Reason = reason;
            DeactivationDate = DateTime.UtcNow;
            EffectiveTo = rule.EffectiveTo;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public string DeactivatedBy { get; }
        public string Reason { get; }
        public DateTime DeactivationDate { get; }
        public DateTime? EffectiveTo { get; }
    }
}
