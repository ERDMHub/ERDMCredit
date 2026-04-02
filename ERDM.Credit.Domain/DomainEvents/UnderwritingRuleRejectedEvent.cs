using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleRejectedEvent : DomainEventBase
    {
        public UnderwritingRuleRejectedEvent(UnderwritingRule rule, string rejectedBy, string reason)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            RejectedBy = rejectedBy;
            Reason = reason;
            RejectionDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public string RejectedBy { get; }
        public string Reason { get; }
        public DateTime RejectionDate { get; }
    }
}
