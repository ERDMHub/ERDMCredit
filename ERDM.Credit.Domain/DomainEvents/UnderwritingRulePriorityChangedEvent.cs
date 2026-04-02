using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a rule's priority changes
    public class UnderwritingRulePriorityChangedEvent : DomainEventBase
    {
        public UnderwritingRulePriorityChangedEvent(UnderwritingRule rule, int previousPriority, int newPriority, string changedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            PreviousPriority = previousPriority;
            NewPriority = newPriority;
            ChangedBy = changedBy;
            ChangeDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public int PreviousPriority { get; }
        public int NewPriority { get; }
        public string ChangedBy { get; }
        public DateTime ChangeDate { get; }
    }
}
