using ERDM.Core.Entities;
using ERDM.Credit.Domain.DomainEvents;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Entities
{
    public class CreditDecision : BaseEntity, IAggregateRoot
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

        // Public methods to raise domain events
        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new CreditDecisionCreatedEvent(this));
        }

        public void RaiseApprovedEvent(string approvedBy)
        {
            var previousStatus = Status;
            var previousType = DecisionType;

            Status = DecisionStatus.Completed;
            DecisionType = DecisionType.Approved;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = approvedBy;

            AddDomainEvent(new CreditDecisionStatusChangedEvent(this, previousStatus, Status, approvedBy));
            AddDomainEvent(new CreditDecisionApprovedEvent(this, approvedBy));
        }

        public void RaiseDeclinedEvent(string declinedBy)
        {
            var previousStatus = Status;
            var previousType = DecisionType;

            Status = DecisionStatus.Completed;
            DecisionType = DecisionType.Declined;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = declinedBy;

            AddDomainEvent(new CreditDecisionStatusChangedEvent(this, previousStatus, Status, declinedBy));
            AddDomainEvent(new CreditDecisionDeclinedEvent(this, declinedBy));
        }

        public void RaiseCounterOfferAcceptedEvent(string acceptedBy)
        {
            var previousStatus = Status;

            Status = DecisionStatus.Accepted;
            DecisionType = DecisionType.Approved;
            ApprovedAmount = CounterOfferAmount;
            ApprovedInterestRate = CounterOfferInterestRate;
            ApprovedTermMonths = CounterOfferTermMonths;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = acceptedBy;

            AddDomainEvent(new CounterOfferAcceptedEvent(this, acceptedBy));
            AddDomainEvent(new CreditDecisionStatusChangedEvent(this, previousStatus, Status, acceptedBy));
            AddDomainEvent(new CreditDecisionApprovedEvent(this, acceptedBy));
        }

        public void RaiseStatusChangedEvent(DecisionStatus newStatus, string changedBy)
        {
            var previousStatus = Status;
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = changedBy;

            AddDomainEvent(new CreditDecisionStatusChangedEvent(this, previousStatus, newStatus, changedBy));
        }

        public void RaiseConditionsAddedEvent(List<UnderwritingCondition> newConditions, string addedBy)
        {
            Conditions = Conditions ?? new List<UnderwritingCondition>();
            Conditions.AddRange(newConditions);
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = addedBy;

            AddDomainEvent(new UnderwritingConditionsAddedEvent(this, newConditions, addedBy));
        }

        public void RaiseConditionMetEvent(string conditionId, string metBy)
        {
            var condition = Conditions?.FirstOrDefault(c => c.ConditionId == conditionId);
            if (condition != null)
            {
                condition.IsMet = true;
                condition.MetDate = DateTime.UtcNow;
                condition.MetBy = metBy;

                AddDomainEvent(new UnderwritingConditionMetEvent(this, condition, metBy));

                // Check if all conditions are met
                var allConditionsMet = Conditions?.All(c => c.IsMet) ?? false;
                if (allConditionsMet)
                {
                    AddDomainEvent(new AllConditionsMetEvent(this));
                }
            }
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = metBy;
        }

        public void RaiseAutoApprovedEvent(string approvalRule)
        {
            var previousStatus = Status;

            Status = DecisionStatus.Completed;
            DecisionType = DecisionType.Approved;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = "AutoApprovalSystem";

            if (Metadata == null)
                Metadata = new DecisionMetadata();

            Metadata.IsAutoApproved = true;
            Metadata.AutoApprovalDate = DateTime.UtcNow;

            AddDomainEvent(new CreditDecisionAutoApprovedEvent(this, approvalRule));
            AddDomainEvent(new CreditDecisionStatusChangedEvent(this, previousStatus, Status, "AutoApprovalSystem"));
            AddDomainEvent(new CreditDecisionApprovedEvent(this, "AutoApprovalSystem"));
        }

        public void RaiseApprovalStepCompletedEvent(int stepNumber, string approverId, string comments)
        {
            var step = ApprovalSteps?.FirstOrDefault(s => s.StepNumber == stepNumber);
            if (step != null)
            {
                step.Status = ApprovalStepStatus.Approved;
                step.ApproverId = approverId;
                step.ApprovedDate = DateTime.UtcNow;
                step.Comments = comments;

                AddDomainEvent(new ApprovalStepCompletedEvent(this, step, approverId));
            }
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = approverId;
        }

        public void RaiseConditionsUpdatedEvent(List<UnderwritingCondition> newConditions, List<UnderwritingCondition> updatedConditions, string updatedBy)
        {
            // Add new conditions
            var addedConditions = newConditions.Where(nc => !Conditions?.Any(c => c.ConditionId == nc.ConditionId) ?? true).ToList();
            if (addedConditions.Any())
            {
                Conditions = Conditions ?? new List<UnderwritingCondition>();
                Conditions.AddRange(addedConditions);
                AddDomainEvent(new UnderwritingConditionsAddedEvent(this, addedConditions, updatedBy));
            }

            // Update existing conditions
            foreach (var updatedCondition in updatedConditions)
            {
                var existingCondition = Conditions?.FirstOrDefault(c => c.ConditionId == updatedCondition.ConditionId);
                if (existingCondition != null)
                {
                    var wasMet = existingCondition.IsMet;
                    existingCondition.Description = updatedCondition.Description;
                    existingCondition.DueDate = updatedCondition.DueDate;
                    existingCondition.ConditionType = updatedCondition.ConditionType;

                    // If condition status changed to met, raise condition met event
                    if (!wasMet && updatedCondition.IsMet)
                    {
                        existingCondition.IsMet = true;
                        existingCondition.MetDate = DateTime.UtcNow;
                        existingCondition.MetBy = updatedBy;
                        AddDomainEvent(new UnderwritingConditionMetEvent(this, existingCondition, updatedBy));
                    }
                }
            }

            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }
    }
}