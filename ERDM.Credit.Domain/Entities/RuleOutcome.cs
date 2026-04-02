
namespace ERDM.Credit.Domain.Entities
{
    public class RuleOutcome
    {
        public string OutcomeType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new();
        public List<string> NextRules { get; set; } = new();
    }
}
