namespace ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos
{
    public class CreateRuleOutcomeDto
    {
        public string OutcomeType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object>? Data { get; set; }
        public List<string>? NextRules { get; set; }
    }
}
