using ERDM.Core.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Entities
{
    public class CreditDecision : BaseEntity
    {
        public string DecisionId { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;

        // Decision Details
        public DecisionType DecisionType { get; set; }
        public DecisionStatus Status { get; set; }
        public DateTime DecisionDate { get; set; }
        public string DecisionBy { get; set; } = string.Empty;

        // Approval Details
        public decimal? ApprovedAmount { get; set; }
        public decimal? ApprovedInterestRate { get; set; }
        public int? ApprovedTermMonths { get; set; }
        public string? ApprovedProductType { get; set; }

        // Decline Details
        public List<string>? DeclineReasons { get; set; }
        public string? DeclineComments { get; set; }

        // Counter Offer
        public bool IsCounterOffer { get; set; }
        public decimal? CounterOfferAmount { get; set; }
        public decimal? CounterOfferInterestRate { get; set; }
        public int? CounterOfferTermMonths { get; set; }
        public DateTime? CounterOfferExpiryDate { get; set; }

        // Risk Assessment
        public string? RiskGrade { get; set; }
        public int? RiskScore { get; set; }
        public string? RiskCategory { get; set; }

        // Underwriting
        public string? UnderwriterId { get; set; }
        public string? UnderwriterComments { get; set; }
        public List<UnderwritingCondition>? Conditions { get; set; }

        // Approval Workflow
        public int ApprovalLevel { get; set; }
        public List<ApprovalStep>? ApprovalSteps { get; set; }

        // Decision Metadata
        public DecisionMetadata Metadata { get; set; } = new();
        public List<string>? Tags { get; set; }
        public string? Notes { get; set; }
    }
}
