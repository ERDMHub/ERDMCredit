using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class CreditApplicationSubmittedEvent : DomainEventBase
    {
        public CreditApplicationSubmittedEvent(CreditApplication application)
        {
            EntityId = application.Id;
            EntityType = nameof(CreditApplication);
            ApplicationId = application.ApplicationId;
            CustomerId = application.CustomerId;
            RequestedAmount = application.RequestedAmount;
        }

        public string ApplicationId { get; }
        public string CustomerId { get; }
        public decimal RequestedAmount { get; }
    }
}
