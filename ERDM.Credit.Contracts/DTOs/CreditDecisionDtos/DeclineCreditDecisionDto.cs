namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class DeclineCreditDecisionDto
    {
        public string DeclinedBy { get; set; } = string.Empty;
        public List<string> DeclineReasons { get; set; } = new();
        public string? Comments { get; set; }
    }

}
