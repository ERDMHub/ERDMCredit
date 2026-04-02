
namespace ERDM.Credit.Contracts.DTOs.UnderwritingRuleDtos
{
    public class RuleActionDto
    {
        public string ActionType { get; set; } = string.Empty;
        public string ActionValue { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
    }
}
