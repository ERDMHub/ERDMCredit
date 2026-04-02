using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleDependencyRemovedEvent : DomainEventBase
    {
        public UnderwritingRuleDependencyRemovedEvent(UnderwritingRule rule, string dependencyRuleId)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            DependencyRuleId = dependencyRuleId;
            RemovedDate = DateTime.UtcNow;
            RemovedBy = rule.UpdatedBy ?? "system";
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string DependencyRuleId { get; }
        public DateTime RemovedDate { get; }
        public string RemovedBy { get; }
    }
}
