using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class CreditDecisionDeclinedEvent : DomainEventBase
    {
        public CreditDecisionDeclinedEvent(CreditDecision decision, string declinedBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            DeclinedBy = declinedBy;
            DeclineReasons = decision.DeclineReasons ?? new List<string>();
            DeclineComments = decision.DeclineComments;
            DeclineDate = DateTime.UtcNow;
            RiskGrade = decision.RiskGrade;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string DeclinedBy { get; }
        public List<string> DeclineReasons { get; }
        public string? DeclineComments { get; }
        public DateTime DeclineDate { get; }
        public string? RiskGrade { get; }
    }
}
