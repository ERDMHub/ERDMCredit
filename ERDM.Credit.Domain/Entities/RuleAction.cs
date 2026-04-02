namespace ERDM.Credit.Domain.Entities
{
    public class RuleAction
    {
        public string ActionType { get; set; } = string.Empty;
        public string ActionValue { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
    }
}
