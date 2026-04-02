using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a risk score is invalidated
    public class RiskScoreInvalidatedEvent : DomainEventBase
    {
        public RiskScoreInvalidatedEvent(RiskScore riskScore, string invalidatedBy, string reason)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            InvalidatedBy = invalidatedBy;
            Reason = reason;
            InvalidationDate = DateTime.UtcNow;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public string InvalidatedBy { get; }
        public string Reason { get; }
        public DateTime InvalidationDate { get; }
    }

}