
namespace ERDM.Credit.Domain.Entities
{
    public class UpdateRuleData
    {
        public string? RuleName { get; set; }
        public string? Condition { get; set; }
        public string? ConditionExpression { get; set; }
        public int? Priority { get; set; }
        public string? Description { get; set; }
        public List<RuleAction>? Actions { get; set; }
        public RuleOutcome? TrueOutcome { get; set; }
        public RuleOutcome? FalseOutcome { get; set; }
    }

}
