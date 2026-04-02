using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.DomainEvents
{ // Event raised when risk threshold is breached
    public class RiskThresholdBreachedEvent : DomainEventBase
    {
        public RiskThresholdBreachedEvent(RiskScore riskScore, string thresholdType, int thresholdValue, int actualValue)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            AccountId = riskScore.AccountId;
            ThresholdType = thresholdType;
            ThresholdValue = thresholdValue;
            ActualValue = actualValue;
            RiskCategory = riskScore.RiskCategory;
            BreachDate = DateTime.UtcNow;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public string? AccountId { get; }
        public string ThresholdType { get; }
        public int ThresholdValue { get; }
        public int ActualValue { get; }
        public RiskCategory RiskCategory { get; }
        public DateTime BreachDate { get; }
    }
}