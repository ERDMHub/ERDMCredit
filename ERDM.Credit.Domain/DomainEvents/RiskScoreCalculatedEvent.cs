using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a risk score is calculated
    public class RiskScoreCalculatedEvent : DomainEventBase
    {
        public RiskScoreCalculatedEvent(RiskScore riskScore)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            ApplicationId = riskScore.ApplicationId;
            AccountId = riskScore.AccountId;
            ScoreType = riskScore.ScoreType;
            ScoreValue = riskScore.ScoreValue;
            ScoreGrade = riskScore.ScoreGrade;
            RiskCategory = riskScore.RiskCategory;
            ProbabilityOfDefault = riskScore.ProbabilityOfDefault;
            ScoringModel = riskScore.ScoringModel;
            ScoringDate = riskScore.ScoringDate;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public string? ApplicationId { get; }
        public string? AccountId { get; }
        public ScoreType ScoreType { get; }
        public int ScoreValue { get; }
        public string ScoreGrade { get; }
        public RiskCategory RiskCategory { get; }
        public decimal ProbabilityOfDefault { get; }
        public string ScoringModel { get; }
        public DateTime ScoringDate { get; }
    }

   
   
}