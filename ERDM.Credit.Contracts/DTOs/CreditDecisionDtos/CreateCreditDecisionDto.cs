namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class CreateCreditDecisionDto
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string DecisionType { get; set; } = string.Empty;
        public string DecisionBy { get; set; } = string.Empty;
        public decimal? ApprovedAmount { get; set; }
        public decimal? ApprovedInterestRate { get; set; }
        public int? ApprovedTermMonths { get; set; }
        public string? ApprovedProductType { get; set; }
        public List<string>? DeclineReasons { get; set; }
        public string? DeclineComments { get; set; }
        public bool IsCounterOffer { get; set; }
        public decimal? CounterOfferAmount { get; set; }
        public decimal? CounterOfferInterestRate { get; set; }
        public int? CounterOfferTermMonths { get; set; }
        public DateTime? CounterOfferExpiryDate { get; set; }
        public string? RiskGrade { get; set; }
        public int? RiskScore { get; set; }
        public string? RiskCategory { get; set; }
        public string? UnderwriterId { get; set; }
        public string? UnderwriterComments { get; set; }
        public List<CreateUnderwritingConditionDto>? Conditions { get; set; }
        public List<CreateApprovalStepDto>? ApprovalSteps { get; set; }
        public string? Notes { get; set; }
        public List<string>? Tags { get; set; }
    }
}
