namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class CreditDecisionResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string DecisionId { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string DecisionType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DecisionDate { get; set; }
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
        public List<UnderwritingConditionDto>? Conditions { get; set; }
        public int ApprovalLevel { get; set; }
        public List<ApprovalStepDto>? ApprovalSteps { get; set; }
        public DecisionMetadataDto Metadata { get; set; } = new();
        public List<string>? Tags { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}
