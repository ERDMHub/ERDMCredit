using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when approval step is completed
    public class ApprovalStepCompletedEvent : DomainEventBase
    {
        public ApprovalStepCompletedEvent(CreditDecision decision, ApprovalStep step, string completedBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            StepNumber = step.StepNumber;
            ApproverRole = step.ApproverRole;
            ApproverId = step.ApproverId;
            CompletedBy = completedBy;
            CompletedDate = step.ApprovedDate ?? DateTime.UtcNow;
            Comments = step.Comments;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public int StepNumber { get; }
        public string ApproverRole { get; }
        public string? ApproverId { get; }
        public string CompletedBy { get; }
        public DateTime CompletedDate { get; }
        public string? Comments { get; }
    }

}
