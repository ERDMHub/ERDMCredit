using ERDM.Core.Entities;
using ERDM.Credit.Domain.DomainEvents;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class LimitHistory : BaseEntity, IAggregateRoot
    {
        [BsonElement("limitHistoryId")]
        public string LimitHistoryId { get; set; } 
        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("accountId")]
        public string AccountId { get; set; }

        [BsonElement("accountNumber")]
        public string AccountNumber { get; set; } 

        [BsonElement("changeType")]
        public LimitChangeType ChangeType { get; set; } 

        [BsonElement("previousLimit")]
        public decimal PreviousLimit { get; set; } 

        [BsonElement("newLimit")]
        public decimal NewLimit { get; set; } 

        [BsonElement("changeAmount")]
        public decimal ChangeAmount { get; set; }

        [BsonElement("changePercentage")]
        public decimal ChangePercentage { get; set; }

        [BsonElement("reason")]
        public string Reason { get; set; }

        [BsonElement("reasonCode")]
        public string ReasonCode { get; set; }

        [BsonElement("changedBy")]
        public string ChangedBy { get; set; }

        [BsonElement("changedDate")]
        public DateTime ChangedDate { get; set; }

        [BsonElement("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [BsonElement("expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        [BsonElement("isTemporary")]
        public bool IsTemporary { get; set; }

        [BsonElement("temporaryDurationDays")]
        public int? TemporaryDurationDays { get; set; }
        [BsonElement("approvalReference")]
        public string? ApprovalReference { get; set; }

        [BsonElement("relatedDecisionId")]
        public string? RelatedDecisionId { get; set; }

        [BsonElement("relatedApplicationId")]
        public string? RelatedApplicationId { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

        [BsonElement("metadata")]
        public LimitHistoryMetadata Metadata { get; set; }

        [BsonElement("tags")]
        public List<string> Tags { get; set; }

        public LimitHistory()
        {
            Tags = new List<string>();
            Metadata = new LimitHistoryMetadata();
        }

        public LimitHistory(
            string customerId,
            string accountId,
            string accountNumber,
            LimitChangeType changeType,
            decimal previousLimit,
            decimal newLimit,
            string reason,
            string reasonCode,
            string changedBy,
            DateTime effectiveDate,
            bool isTemporary = false,
            int? temporaryDurationDays = null,
            string? approvalReference = null,
            string? notes = null) : this()
        {
            LimitHistoryId = GenerateLimitHistoryId();
            CustomerId = customerId;
            AccountId = accountId;
            AccountNumber = accountNumber;
            ChangeType = changeType;
            PreviousLimit = previousLimit;
            NewLimit = newLimit;
            ChangeAmount = newLimit - previousLimit;
            ChangePercentage = previousLimit > 0 ? (ChangeAmount / previousLimit) * 100 : 0;
            Reason = reason;
            ReasonCode = reasonCode;
            ChangedBy = changedBy;
            ChangedDate = DateTime.UtcNow;
            EffectiveDate = effectiveDate;
            IsTemporary = isTemporary;
            TemporaryDurationDays = temporaryDurationDays;
            ApprovalReference = approvalReference;
            Notes = notes;

            if (isTemporary && temporaryDurationDays.HasValue)
            {
                ExpiryDate = effectiveDate.AddDays(temporaryDurationDays.Value);
            }

            MarkAsCreated(changedBy);
        }

        // Public method to raise created event (called from service)
        public void RaiseCreatedEvent()
        {
            // Raise appropriate event based on change type
            if (ChangeType == LimitChangeType.Increase || ChangeType == LimitChangeType.TemporaryIncrease)
            {
                AddDomainEvent(new LimitIncreasedEvent(this));
            }
            else if (ChangeType == LimitChangeType.Decrease || ChangeType == LimitChangeType.TemporaryDecrease)
            {
                AddDomainEvent(new LimitDecreasedEvent(this));
            }
        }

        // Public method to update expiry date (called from service)
        public void UpdateExpiryDate(DateTime newExpiryDate, string updatedBy)
        {
            var oldExpiryDate = ExpiryDate;
            ExpiryDate = newExpiryDate;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;

            AddDomainEvent(new LimitHistoryExtendedEvent(this, newExpiryDate, updatedBy));
        }

        // Public method to revert limit (called from service)
        public void RevertLimit(decimal revertedLimit, string revertedBy, string revertReason)
        {
            var previousLimit = NewLimit;
            var newLimit = revertedLimit;

            NewLimit = newLimit;
            ChangeAmount = newLimit - previousLimit;
            ChangePercentage = previousLimit > 0 ? (ChangeAmount / previousLimit) * 100 : 0;
            Reason = revertReason;
            ChangedBy = revertedBy;
            ChangedDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = revertedBy;

            AddDomainEvent(new LimitHistoryRevertedEvent(this, previousLimit, newLimit, revertedBy, revertReason));
        }

        // Public method to expire temporary limit (called from service)
        public void ExpireTemporaryLimit(string expiredBy)
        {
            if (!IsTemporary)
                return;

            var expiredLimit = NewLimit;
            var originalLimit = PreviousLimit;

            // Revert to original limit
            NewLimit = PreviousLimit;
            ChangeAmount = NewLimit - expiredLimit;
            ChangePercentage = expiredLimit > 0 ? (ChangeAmount / expiredLimit) * 100 : 0;
            Reason = "Temporary limit expired";
            ChangedBy = expiredBy;
            ChangedDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = expiredBy;
            IsTemporary = false;
            ExpiryDate = null;

            AddDomainEvent(new TemporaryLimitExpiredEvent(this, DateTime.UtcNow));
        }

        // Public method to add metadata
        public void AddMetadata(string key, object value)
        {
            Metadata.AdditionalData ??= new Dictionary<string, object>();
            Metadata.AdditionalData[key] = value;
        }

        private string GenerateLimitHistoryId()
        {
            return $"LH-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }

    public class LimitHistoryMetadata
    {
        public string? Source { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? WorkflowId { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}