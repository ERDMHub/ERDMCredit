using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class CreditApplicationApprovedEvent : DomainEventBase
    {
        public CreditApplicationApprovedEvent(CreditApplication application)
        {
            EntityId = application.Id;
            EntityType = nameof(CreditApplication);
            ApplicationId = application.ApplicationId;
            ApprovedAmount = application.Decision.ApprovedAmount.Value;
            InterestRate = application.Decision.InterestRate.Value;
        }

        public string ApplicationId { get; }
        public decimal ApprovedAmount { get; }
        public decimal InterestRate { get; }
    }
}
