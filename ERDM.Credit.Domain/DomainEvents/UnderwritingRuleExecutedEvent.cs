using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class UnderwritingRuleExecutedEvent : DomainEventBase
    {
        public UnderwritingRuleExecutedEvent(UnderwritingRule rule, bool succeeded, string? failureReason)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            RuleCode = rule.RuleCode;
            Succeeded = succeeded;
            FailureReason = failureReason;
            ExecutionDate = DateTime.UtcNow;
            TotalExecutions = rule.ExecutionCount;
            SuccessRate = rule.SuccessRate;
            ExecutionTimeMs = 0; // Would be calculated in service
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public string RuleCode { get; }
        public bool Succeeded { get; }
        public string? FailureReason { get; }
        public DateTime ExecutionDate { get; }
        public int TotalExecutions { get; }
        public decimal SuccessRate { get; }
        public long ExecutionTimeMs { get; }
    }
}
