namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class UnderwritingConditionDto
    {
        public string ConditionId { get; set; } = string.Empty;
        public string ConditionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsMet { get; set; }
        public DateTime? MetDate { get; set; }
        public string? MetBy { get; set; }
        public DateTime DueDate { get; set; }
    }
}
