namespace ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos
{
    public class UpdateRuleActionDto
    {
        public string ActionType { get; set; } = string.Empty;
        public string ActionValue { get; set; } = string.Empty;
        public Dictionary<string, object>? Parameters { get; set; }
    }
}
