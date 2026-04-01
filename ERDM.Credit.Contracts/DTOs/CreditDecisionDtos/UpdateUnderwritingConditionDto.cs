namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class UpdateUnderwritingConditionDto
    {
        public string ConditionId { get; set; } = string.Empty;
        public bool IsMet { get; set; }
        public string? MetBy { get; set; }
        public string? Comments { get; set; }
    }
}
