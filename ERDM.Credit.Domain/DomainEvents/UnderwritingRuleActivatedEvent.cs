using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleActivatedEvent : DomainEventBase
    {
        public UnderwritingRuleActivatedEvent(UnderwritingRule rule, string activatedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            ActivatedBy = activatedBy;
            ActivationDate = DateTime.UtcNow;
            EffectiveFrom = rule.EffectiveFrom;
            RuleVersion = rule.RuleVersion;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public string ActivatedBy { get; }
        public DateTime ActivationDate { get; }
        public DateTime EffectiveFrom { get; }
        public int RuleVersion { get; }
    }

}
