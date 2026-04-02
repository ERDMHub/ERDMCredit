using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
 // Event raised when a risk factor is added
    public class RiskFactorAddedEvent : DomainEventBase
    {
        public RiskFactorAddedEvent(RiskScore riskScore, RiskFactor riskFactor)
        {
            EntityId = riskScore.Id;
            EntityType = nameof(RiskScore);
            RiskScoreId = riskScore.RiskScoreId;
            CustomerId = riskScore.CustomerId;
            FactorId = riskFactor.FactorId;
            FactorName = riskFactor.FactorName;
            FactorValue = riskFactor.FactorValue;
            Weight = riskFactor.Weight;
            Impact = riskFactor.Impact;
        }

        public string RiskScoreId { get; }
        public string CustomerId { get; }
        public string FactorId { get; }
        public string FactorName { get; }
        public string FactorValue { get; }
        public decimal Weight { get; }
        public string Impact { get; }
    }
}