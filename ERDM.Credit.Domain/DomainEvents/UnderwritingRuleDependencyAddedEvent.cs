using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleDependencyAddedEvent : DomainEventBase
    {
        public UnderwritingRuleDependencyAddedEvent(UnderwritingRule rule, string dependencyRuleId)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            DependencyRuleId = dependencyRuleId;
            AddedDate = DateTime.UtcNow;
            AddedBy = rule.UpdatedBy ?? "system";
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string DependencyRuleId { get; }
        public DateTime AddedDate { get; }
        public string AddedBy { get; }
    }

}
