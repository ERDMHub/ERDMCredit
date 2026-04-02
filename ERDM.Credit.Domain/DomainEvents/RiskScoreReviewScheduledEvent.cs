using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
     // Event raised when a risk score review is scheduled
    public class RiskScoreReviewScheduledEvent : DomainEventBase
    {
        public RiskScoreReviewScheduledEvent(RiskScore riskScore, DateTime reviewDate, string scheduledBy)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            ReviewDate = reviewDate;
            ScheduledBy = scheduledBy;
            ScheduledDate = DateTime.UtcNow;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public DateTime ReviewDate { get; }
        public string ScheduledBy { get; }
        public DateTime ScheduledDate { get; }
    }

}