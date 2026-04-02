
namespace ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos
{
    public class UpdateUnderwritingRuleDto
    {
        public string? RuleName { get; set; }
        public string? Condition { get; set; }
        public string? ConditionExpression { get; set; }
        public int? Priority { get; set; }
        public string? Description { get; set; }
        public List<UpdateRuleActionDto>? Actions { get; set; }
        public UpdateRuleOutcomeDto? TrueOutcome { get; set; }
        public UpdateRuleOutcomeDto? FalseOutcome { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }

}
