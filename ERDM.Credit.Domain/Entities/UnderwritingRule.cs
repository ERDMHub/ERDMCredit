using ERDM.Core.Entities;
using ERDM.Credit.Domain.DomainEvents;
using ERDM.Credit.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class UnderwritingRule : BaseEntity, IAggregateRoot
    {
        [BsonElement("ruleId")]
        public string RuleId { get; set; }

        [BsonElement("ruleName")]
        public string RuleName { get; set; }

        [BsonElement("ruleCode")]
        public string RuleCode { get; set; }

        [BsonElement("ruleType")]
        public RuleType RuleType { get; set; }

        [BsonElement("category")]
        public RuleCategory Category { get; set; }

        [BsonElement("priority")]
        public int Priority { get; set; }

        [BsonElement("condition")]
        public string Condition { get; set; }

        [BsonElement("conditionExpression")]
        public string ConditionExpression { get; set; }

        [BsonElement("actions")]
        public List<RuleAction> Actions { get; set; }

        [BsonElement("trueOutcome")]
        public RuleOutcome TrueOutcome { get; set; }

        [BsonElement("falseOutcome")]
        public RuleOutcome? FalseOutcome { get; set; }

        [BsonElement("status")]
        public RuleStatus Status { get; set; }

        [BsonElement("version")]
        public int RuleVersion { get; set; }

        [BsonElement("effectiveFrom")]
        public DateTime EffectiveFrom { get; set; }

        [BsonElement("effectiveTo")]
        public DateTime? EffectiveTo { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("successRate")]
        public decimal SuccessRate { get; set; }

        [BsonElement("executionCount")]
        public int ExecutionCount { get; set; }

        [BsonElement("lastExecutedAt")]
        public DateTime? LastExecutedAt { get; set; }

        [BsonElement("createdBy")]
        public string CreatedBy { get; set; }

        [BsonElement("approvedBy")]
        public string? ApprovedBy { get; set; }

        [BsonElement("approvedAt")]
        public DateTime? ApprovedAt { get; set; }

        [BsonElement("dependsOnRules")]
        public List<string> DependsOnRules { get; set; }

        [BsonElement("metadata")]
        public UnderwritingRuleMetadata Metadata { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }

        public UnderwritingRule()
        {
            Actions = new List<RuleAction>();
            DependsOnRules = new List<string>();
            Tags = new List<string>();
            Metadata = new UnderwritingRuleMetadata();
        }

        public UnderwritingRule(
            string ruleName,
            string ruleCode,
            RuleType ruleType,
            RuleCategory category,
            int priority,
            string condition,
            string conditionExpression,
            List<RuleAction> actions,
            RuleOutcome trueOutcome,
            string description,
            string createdBy,
            RuleOutcome? falseOutcome = null) : this()
        {
            RuleId = GenerateRuleId();
            RuleName = ruleName;
            RuleCode = ruleCode;
            RuleType = ruleType;
            Category = category;
            Priority = priority;
            Condition = condition;
            ConditionExpression = conditionExpression;
            Actions = actions;
            TrueOutcome = trueOutcome;
            FalseOutcome = falseOutcome;
            Description = description;
            CreatedBy = createdBy;
            Status = RuleStatus.Draft;
            RuleVersion = 1;
            EffectiveFrom = DateTime.UtcNow;
            SuccessRate = 0;
            ExecutionCount = 0;

            MarkAsCreated(createdBy);
        }

        // Public methods to raise events
        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new UnderwritingRuleCreatedEvent(this));
        }

        public void Activate(string activatedBy)
        {
            if (Status != RuleStatus.Draft && Status != RuleStatus.Inactive)
                throw new InvalidOperationException($"Cannot activate rule from {Status} status");

            Status = RuleStatus.Active;
            EffectiveFrom = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = activatedBy;

            AddDomainEvent(new UnderwritingRuleActivatedEvent(this, activatedBy));
        }

        public void Deactivate(string deactivatedBy, string reason)
        {
            if (Status != RuleStatus.Active)
                throw new InvalidOperationException($"Cannot deactivate rule from {Status} status");

            Status = RuleStatus.Inactive;
            EffectiveTo = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = deactivatedBy;
            Metadata.DeactivationReason = reason;

            AddDomainEvent(new UnderwritingRuleDeactivatedEvent(this, deactivatedBy, reason));
        }

        public void Approve(string approvedBy)
        {
            if (Status != RuleStatus.Draft)
                throw new InvalidOperationException($"Only draft rules can be approved");

            Status = RuleStatus.Approved;
            ApprovedBy = approvedBy;
            ApprovedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = approvedBy;

            AddDomainEvent(new UnderwritingRuleApprovedEvent(this, approvedBy));
        }

        public void Reject(string rejectedBy, string reason)
        {
            if (Status != RuleStatus.Draft)
                throw new InvalidOperationException($"Only draft rules can be rejected");

            Status = RuleStatus.Rejected;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = rejectedBy;
            Metadata.RejectionReason = reason;

            AddDomainEvent(new UnderwritingRuleRejectedEvent(this, rejectedBy, reason));
        }

        public void UpdateRule(UpdateRuleData updateData, string updatedBy)
        {
            var previousVersion = RuleVersion;

            RuleName = updateData.RuleName ?? RuleName;
            Condition = updateData.Condition ?? Condition;
            ConditionExpression = updateData.ConditionExpression ?? ConditionExpression;
            Priority = updateData.Priority ?? Priority;
            Description = updateData.Description ?? Description;

            if (updateData.Actions != null)
                Actions = updateData.Actions;

            if (updateData.TrueOutcome != null)
                TrueOutcome = updateData.TrueOutcome;

            if (updateData.FalseOutcome != null)
                FalseOutcome = updateData.FalseOutcome;

            RuleVersion++;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;

            AddDomainEvent(new UnderwritingRuleUpdatedEvent(this, previousVersion, RuleVersion, updatedBy));
        }

        public void RecordExecution(bool succeeded, string? failureReason = null)
        {
            ExecutionCount++;
            LastExecutedAt = DateTime.UtcNow;

            // Update success rate
            var totalSuccess = (SuccessRate * (ExecutionCount - 1) + (succeeded ? 100 : 0)) / ExecutionCount;
            SuccessRate = Math.Round(totalSuccess, 2);

            if (!succeeded)
            {
                Metadata.LastFailureReason = failureReason;
                Metadata.LastFailureAt = DateTime.UtcNow;
            }

            AddDomainEvent(new UnderwritingRuleExecutedEvent(this, succeeded, failureReason));
        }

        public void AddDependency(string ruleId)
        {
            if (!DependsOnRules.Contains(ruleId))
            {
                DependsOnRules.Add(ruleId);
                AddDomainEvent(new UnderwritingRuleDependencyAddedEvent(this, ruleId));
            }
        }

        public void RemoveDependency(string ruleId)
        {
            if (DependsOnRules.Remove(ruleId))
            {
                AddDomainEvent(new UnderwritingRuleDependencyRemovedEvent(this, ruleId));
            }
        }

        public void ScheduleEffectiveDate(DateTime effectiveFrom, DateTime? effectiveTo, string scheduledBy)
        {
            EffectiveFrom = effectiveFrom;
            EffectiveTo = effectiveTo;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = scheduledBy;

            AddDomainEvent(new UnderwritingRuleScheduledEvent(this, effectiveFrom, effectiveTo, scheduledBy));
        }

        public void AddMetadata(string key, object value)
        {
            Metadata.AdditionalData ??= new Dictionary<string, object>();
            Metadata.AdditionalData[key] = value;
        }

        private string GenerateRuleId()
        {
            return $"RULE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}