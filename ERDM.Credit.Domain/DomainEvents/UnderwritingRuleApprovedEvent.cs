using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleApprovedEvent : DomainEventBase
    {
        public UnderwritingRuleApprovedEvent(UnderwritingRule rule, string approvedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            ApprovedBy = approvedBy;
            ApprovalDate = DateTime.UtcNow;
            RuleVersion = rule.RuleVersion;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public string ApprovedBy { get; }
        public DateTime ApprovalDate { get; }
        public int RuleVersion { get; }
    }

}
