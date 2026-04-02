
namespace ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos
{
    public class CreateUnderwritingRuleDto
    {
        public string RuleName { get; set; } = string.Empty;
        public string RuleCode { get; set; } = string.Empty;
        public string RuleType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string ConditionExpression { get; set; } = string.Empty;
        public List<CreateRuleActionDto> Actions { get; set; } = new();
        public CreateRuleOutcomeDto TrueOutcome { get; set; } = new();
        public CreateRuleOutcomeDto? FalseOutcome { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public List<string>? DependsOnRules { get; set; }
        public List<string>? Tags { get; set; }
    }
}
