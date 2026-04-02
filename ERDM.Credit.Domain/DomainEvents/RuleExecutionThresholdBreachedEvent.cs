using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when rule execution threshold is breached
    public class RuleExecutionThresholdBreachedEvent : DomainEventBase
    {
        public RuleExecutionThresholdBreachedEvent(UnderwritingRule rule, string thresholdType, decimal thresholdValue, decimal actualValue)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            ThresholdType = thresholdType; // SuccessRate, ExecutionCount, etc.
            ThresholdValue = thresholdValue;
            ActualValue = actualValue;
            BreachDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string ThresholdType { get; }
        public decimal ThresholdValue { get; }
        public decimal ActualValue { get; }
        public DateTime BreachDate { get; }
    }
}
