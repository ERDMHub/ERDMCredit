using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{

    // Event raised when a rule fails validation
    public class UnderwritingRuleValidationFailedEvent : DomainEventBase
    {
        public UnderwritingRuleValidationFailedEvent(UnderwritingRule rule, List<string> errors, string validatedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            Errors = errors;
            ValidatedBy = validatedBy;
            ValidationDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public List<string> Errors { get; }
        public string ValidatedBy { get; }
        public DateTime ValidationDate { get; }
    }
}
