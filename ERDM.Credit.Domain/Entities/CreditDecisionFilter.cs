using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Entities
{
    public class CreditDecisionFilter
    {
        public string? ApplicationId { get; set; }
        public string? CustomerId { get; set; }
        public DecisionType? DecisionType { get; set; }
        public DecisionStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? DecisionBy { get; set; }
        public string? RiskGrade { get; set; }
        public bool? IsCounterOffer { get; set; }
    }
}
