using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when all underwriting conditions are satisfied
    public class AllConditionsMetEvent : DomainEventBase
    {
        public AllConditionsMetEvent(CreditDecision decision)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            TotalConditions = decision.Conditions?.Count ?? 0;
            AllConditionsMetDate = DateTime.UtcNow;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public int TotalConditions { get; }
        public DateTime AllConditionsMetDate { get; }
    }
}
