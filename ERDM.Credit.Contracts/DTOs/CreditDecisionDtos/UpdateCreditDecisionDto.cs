namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class UpdateCreditDecisionDto
    {
        public string? DecisionType { get; set; }
        public string? Status { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public decimal? ApprovedInterestRate { get; set; }
        public int? ApprovedTermMonths { get; set; }
        public List<string>? DeclineReasons { get; set; }
        public string? DeclineComments { get; set; }
        public string? UnderwriterComments { get; set; }
        public List<UpdateUnderwritingConditionDto>? Conditions { get; set; }
        public List<UpdateApprovalStepDto>? ApprovalSteps { get; set; }
        public string? Notes { get; set; }
    }
}
