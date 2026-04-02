using ERDM.Core.Entities;
using ERDM.Credit.Domain.DomainEvents;
using ERDM.Credit.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Credit.Domain.Entities
{
    public class RiskScore : BaseEntity, IAggregateRoot
    {
        [BsonElement("riskScoreId")]
        public string RiskScoreId { get;  set; }

        [BsonElement("customerId")]
        public string CustomerId { get;  set; }

        [BsonElement("applicationId")]
        public string? ApplicationId { get;  set; }

        [BsonElement("accountId")]
        public string? AccountId { get;  set; }

        [BsonElement("scoreType")]
        public ScoreType ScoreType { get;  set; }

        [BsonElement("scoreValue")]
        public int ScoreValue { get;  set; }

        [BsonElement("previousScore")]
        public int? PreviousScore { get;  set; }

        [BsonElement("scoreChange")]
        public int ScoreChange { get;  set; }

        [BsonElement("scoreGrade")]
        public string ScoreGrade { get;  set; }

        [BsonElement("riskCategory")]
        public RiskCategory RiskCategory { get;  set; }

        [BsonElement("probabilityOfDefault")]
        public decimal ProbabilityOfDefault { get;  set; }

        [BsonElement("lossGivenDefault")]
        public decimal LossGivenDefault { get;  set; }

        [BsonElement("exposureAtDefault")]
        public decimal ExposureAtDefault { get;  set; }

        [BsonElement("expectedLoss")]
        public decimal ExpectedLoss { get;  set; }

        [BsonElement("riskFactors")]
        public List<RiskFactor> RiskFactors { get;  set; }

        [BsonElement("scoringDate")]
        public DateTime ScoringDate { get;  set; }

        [BsonElement("scoredBy")]
        public string ScoredBy { get;  set; }

        [BsonElement("scoringModel")]
        public string ScoringModel { get;  set; }

        [BsonElement("scoringModelVersion")]
        public string ScoringModelVersion { get;  set; }

        [BsonElement("isValid")]
        public bool IsValid { get;  set; }

        [BsonElement("validUntil")]
        public DateTime? ValidUntil { get;  set; }

        [BsonElement("nextReviewDate")]
        public DateTime? NextReviewDate { get;  set; }

        [BsonElement("reviewNotes")]
        public string? ReviewNotes { get;  set; }

        [BsonElement("metadata")]
        public RiskScoreMetadata Metadata { get;  set; }

        [BsonElement("tags")]
        public List<string> Tags { get;  set; }

        public RiskScore()
        {
            RiskFactors = new List<RiskFactor>();
            Tags = new List<string>();
            Metadata = new RiskScoreMetadata();
        }

        public RiskScore(
            string customerId,
            ScoreType scoreType,
            int scoreValue,
            string scoreGrade,
            RiskCategory riskCategory,
            string scoredBy,
            string scoringModel,
            string scoringModelVersion,
            string? applicationId = null,
            string? accountId = null) : this()
        {
            RiskScoreId = GenerateRiskScoreId();
            CustomerId = customerId;
            ApplicationId = applicationId;
            AccountId = accountId;
            ScoreType = scoreType;
            ScoreValue = scoreValue;
            ScoreGrade = scoreGrade;
            RiskCategory = riskCategory;
            ScoredBy = scoredBy;
            ScoringDate = DateTime.UtcNow;
            ScoringModel = scoringModel;
            ScoringModelVersion = scoringModelVersion;
            IsValid = true;
            ValidUntil = DateTime.UtcNow.AddDays(90);
            NextReviewDate = DateTime.UtcNow.AddDays(30);

            MarkAsCreated(scoredBy);
        }

        // Public methods to raise events
        public void RaiseCreatedEvent()
        {
            AddDomainEvent(new RiskScoreCalculatedEvent(this));
        }

        public void UpdateScore(int newScoreValue, string scoreGrade, RiskCategory riskCategory, string updatedBy)
        {
            PreviousScore = ScoreValue;
            ScoreValue = newScoreValue;
            ScoreChange = newScoreValue - (PreviousScore ?? 0);
            ScoreGrade = scoreGrade;
            RiskCategory = riskCategory;
            ScoringDate = DateTime.UtcNow;
            ScoredBy = updatedBy;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            IsValid = true;
            ValidUntil = DateTime.UtcNow.AddDays(90);
            NextReviewDate = DateTime.UtcNow.AddDays(30);

            AddDomainEvent(new RiskScoreUpdatedEvent(this, PreviousScore.Value, newScoreValue));
        }

        public void AddRiskFactor(string factorName, string factorValue, decimal weight, string impact)
        {
            var riskFactor = new RiskFactor
            {
                FactorId = Guid.NewGuid().ToString(),
                FactorName = factorName,
                FactorValue = factorValue,
                Weight = weight,
                Impact = impact,
                AssessedAt = DateTime.UtcNow
            };
            RiskFactors.Add(riskFactor);
            
            AddDomainEvent(new RiskFactorAddedEvent(this, riskFactor));
        }

        public void UpdateRiskMetrics(decimal probabilityOfDefault, decimal lossGivenDefault, decimal exposureAtDefault)
        {
            ProbabilityOfDefault = probabilityOfDefault;
            LossGivenDefault = lossGivenDefault;
            ExposureAtDefault = exposureAtDefault;
            ExpectedLoss = probabilityOfDefault * lossGivenDefault * exposureAtDefault;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new RiskMetricsUpdatedEvent(this, probabilityOfDefault, lossGivenDefault, exposureAtDefault));
        }

        public void InvalidateScore(string invalidatedBy, string reason)
        {
            IsValid = false;
            ValidUntil = null;
            ReviewNotes = reason;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = invalidatedBy;

            AddDomainEvent(new RiskScoreInvalidatedEvent(this, invalidatedBy, reason));
        }

        public void ScheduleReview(DateTime reviewDate, string scheduledBy)
        {
            NextReviewDate = reviewDate;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = scheduledBy;

            AddDomainEvent(new RiskScoreReviewScheduledEvent(this, reviewDate, scheduledBy));
        }

        public void AddMetadata(string key, object value)
        {
            Metadata.AdditionalData ??= new Dictionary<string, object>();
            Metadata.AdditionalData[key] = value;
        }

        private string GenerateRiskScoreId()
        {
            return $"RS-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}