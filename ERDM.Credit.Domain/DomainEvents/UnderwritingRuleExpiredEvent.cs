using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleExpiredEvent : DomainEventBase
    {
        public UnderwritingRuleExpiredEvent(UnderwritingRule rule)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            ExpiryDate = rule.EffectiveTo ?? DateTime.UtcNow;
            WasActive = rule.Status == RuleStatus.Active;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public DateTime ExpiryDate { get; }
        public bool WasActive { get; }
    }

}
