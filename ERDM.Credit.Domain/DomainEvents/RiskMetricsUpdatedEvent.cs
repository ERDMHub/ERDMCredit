using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
   // Event raised when risk metrics are updated
    public class RiskMetricsUpdatedEvent : DomainEventBase
    {
        public RiskMetricsUpdatedEvent(RiskScore riskScore, decimal probabilityOfDefault, decimal lossGivenDefault, decimal exposureAtDefault)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            ProbabilityOfDefault = probabilityOfDefault;
            LossGivenDefault = lossGivenDefault;
            ExposureAtDefault = exposureAtDefault;
            ExpectedLoss = riskScore.ExpectedLoss;
            UpdateDate = DateTime.UtcNow;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public decimal ProbabilityOfDefault { get; }
        public decimal LossGivenDefault { get; }
        public decimal ExposureAtDefault { get; }
        public decimal ExpectedLoss { get; }
        public DateTime UpdateDate { get; }
    }

}