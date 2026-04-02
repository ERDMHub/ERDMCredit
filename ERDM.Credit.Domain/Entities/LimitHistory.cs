using ERDM.Core.Entities;
using ERDM.Credit.Domain.DomainEvents;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class LimitHistory : BaseEntity, IAggregateRoot
    {
        [BsonElement("limitHistoryId")]
        public string LimitHistoryId { get; private set; }

        [BsonElement("customerId")]
        public string CustomerId { get; private set; }

        [BsonElement("accountId")]
        public string AccountId { get; private set; }

        [BsonElement("accountNumber")]
        public string AccountNumber { get; private set; }

        [BsonElement("changeType")]
        public LimitChangeType ChangeType { get; private set; }

        [BsonElement("previousLimit")]
        public decimal PreviousLimit { get; private set; }

        [BsonElement("newLimit")]
        public decimal NewLimit { get; private set; }

        [BsonElement("changeAmount")]
        public decimal ChangeAmount { get; private set; }

        [BsonElement("changePercentage")]
        public decimal ChangePercentage { get; private set; }

        [BsonElement("reason")]
        public string Reason { get; private set; }

        [BsonElement("reasonCode")]
        public string ReasonCode { get; private set; }

        [BsonElement("changedBy")]
        public string ChangedBy { get; private set; }

        [BsonElement("changedDate")]
        public DateTime ChangedDate { get; private set; }

        [BsonElement("effectiveDate")]
        public DateTime EffectiveDate { get; private set; }

        [BsonElement("expiryDate")]
        public DateTime? ExpiryDate { get; private set; }

        [BsonElement("isTemporary")]
        public bool IsTemporary { get; private set; }

        [BsonElement("temporaryDurationDays")]
        public int? TemporaryDurationDays { get; private set; }

        [BsonElement("approvalReference")]
        public string? ApprovalReference { get; private set; }

        [BsonElement("relatedDecisionId")]
        public string? RelatedDecisionId { get; private set; }

        [BsonElement("relatedApplicationId")]
        public string? RelatedApplicationId { get; private set; }

        [BsonElement("notes")]
        public string? Notes { get; private set; }

        [BsonElement("metadata")]
        public LimitHistoryMetadata Metadata { get; private set; }

        [BsonElement("tags")]
        public List<string> Tags { get; private set; }

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

        public void UpdateExpiryDate(DateTime newExpiryDate, string updatedBy)
        {
            ExpiryDate = newExpiryDate;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            
            AddDomainEvent(new LimitHistoryExtendedEvent(this, newExpiryDate, updatedBy));
        }

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