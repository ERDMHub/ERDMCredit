using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleUpdatedEvent : DomainEventBase
    {
        public UnderwritingRuleUpdatedEvent(UnderwritingRule rule, int previousVersion, int newVersion, string updatedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            PreviousVersion = previousVersion;
            NewVersion = newVersion;
            UpdatedBy = updatedBy;
            UpdateDate = DateTime.UtcNow;
            Changes = GetChanges(rule);
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public int PreviousVersion { get; }
        public int NewVersion { get; }
        public string UpdatedBy { get; }
        public DateTime UpdateDate { get; }
        public Dictionary<string, object> Changes { get; }

        private Dictionary<string, object> GetChanges(UnderwritingRule rule)
        {
            // This would capture what changed - simplified for brevity
            return new Dictionary<string, object>
            {
                ["Condition"] = rule.Condition,
                ["Priority"] = rule.Priority,
                ["Actions"] = rule.Actions?.Count ?? 0
            };
        }
    }

}
