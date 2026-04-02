using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a risk score is updated
    public class RiskScoreUpdatedEvent : DomainEventBase
    {
        public RiskScoreUpdatedEvent(RiskScore riskScore, int previousScore, int newScore)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            PreviousScore = previousScore;
            NewScore = newScore;
            ScoreChange = newScore - previousScore;
            ScoreGrade = riskScore.ScoreGrade;
            RiskCategory = riskScore.RiskCategory;
            UpdateDate = DateTime.UtcNow;
            UpdatedBy = riskScore.ScoredBy;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public int PreviousScore { get; }
        public int NewScore { get; }
        public int ScoreChange { get; }
        public string ScoreGrade { get; }
        public RiskCategory RiskCategory { get; }
        public DateTime UpdateDate { get; }
        public string UpdatedBy { get; }
    }

}
