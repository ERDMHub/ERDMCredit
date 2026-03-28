using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class CreditApplicationDeclinedEvent : DomainEventBase
    {
        public CreditApplicationDeclinedEvent(CreditApplication application)
        {
            EntityId = application.Id;
            EntityType = nameof(CreditApplication);
            ApplicationId = application.ApplicationId;
            DeclineReasons = application.Decision.DeclineReasons;
        }

        public string ApplicationId { get; }
        public List<string> DeclineReasons { get; }
    }
}
