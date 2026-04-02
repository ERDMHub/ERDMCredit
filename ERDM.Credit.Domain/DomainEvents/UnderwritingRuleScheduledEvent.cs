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
    public class UnderwritingRuleScheduledEvent : DomainEventBase
    {
        public UnderwritingRuleScheduledEvent(UnderwritingRule rule, DateTime effectiveFrom, DateTime? effectiveTo, string scheduledBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
            ScheduledBy = scheduledBy;
            ScheduleDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public DateTime EffectiveFrom { get; }
        public DateTime? EffectiveTo { get; }
        public string ScheduledBy { get; }
        public DateTime ScheduleDate { get; }
    }
  
    
   
    // Event raised when rule condition expression is invalid
    
}
