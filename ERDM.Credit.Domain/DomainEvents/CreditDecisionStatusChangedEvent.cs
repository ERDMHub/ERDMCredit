using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when decision status changes
    public class CreditDecisionStatusChangedEvent : DomainEventBase
    {
        public CreditDecisionStatusChangedEvent(CreditDecision decision, DecisionStatus previousStatus, DecisionStatus newStatus, string changedBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            PreviousStatus = previousStatus;
            NewStatus = newStatus;
            ChangedBy = changedBy;
            ChangeDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public DecisionStatus PreviousStatus { get; }
        public DecisionStatus NewStatus { get; }
        public string ChangedBy { get; }
        public DateTime ChangeDate { get; }
    }
}
