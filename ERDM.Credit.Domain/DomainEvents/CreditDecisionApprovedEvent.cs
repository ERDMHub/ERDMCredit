using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class CreditDecisionApprovedEvent : DomainEventBase
    {
        public CreditDecisionApprovedEvent(CreditDecision decision, string approvedBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            ApprovedBy = approvedBy;
            ApprovedAmount = decision.ApprovedAmount;
            ApprovedInterestRate = decision.ApprovedInterestRate;
            ApprovedTermMonths = decision.ApprovedTermMonths;
            ApprovedProductType = decision.ApprovedProductType;
            ApprovalDate = DateTime.UtcNow;
            RiskGrade = decision.RiskGrade;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string ApprovedBy { get; }
        public decimal? ApprovedAmount { get; }
        public decimal? ApprovedInterestRate { get; }
        public int? ApprovedTermMonths { get; }
        public string? ApprovedProductType { get; }
        public DateTime ApprovalDate { get; }
        public string? RiskGrade { get; }
    }
}
