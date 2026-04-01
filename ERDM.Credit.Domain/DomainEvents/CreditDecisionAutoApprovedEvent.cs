using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when decision is auto-approved by the system
    public class CreditDecisionAutoApprovedEvent : DomainEventBase
    {
        public CreditDecisionAutoApprovedEvent(CreditDecision decision, string approvalRule)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            ApprovalRule = approvalRule;
            AutoApprovedAmount = decision.ApprovedAmount;
            AutoApprovalDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string ApprovalRule { get; }
        public decimal? AutoApprovedAmount { get; }
        public DateTime AutoApprovalDate { get; }
    }
}
