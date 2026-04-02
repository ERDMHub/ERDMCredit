using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a rule is tested
    public class UnderwritingRuleTestedEvent : DomainEventBase
    {
        public UnderwritingRuleTestedEvent(UnderwritingRule rule, Dictionary<string, object> testData, bool result, string testedBy)
        {
            EntityId = rule.Id;
            EntityType = nameof(UnderwritingRule);
            RuleId = rule.RuleId;
            RuleName = rule.RuleName;
            TestData = testData;
            Result = result;
            TestedBy = testedBy;
            TestDate = DateTime.UtcNow;
        }

        public string RuleId { get; }
        public string RuleName { get; }
        public Dictionary<string, object> TestData { get; }
        public bool Result { get; }
        public string TestedBy { get; }
        public DateTime TestDate { get; }
    }
}
