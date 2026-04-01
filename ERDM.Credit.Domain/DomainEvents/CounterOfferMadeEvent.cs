using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;


namespace ERDM.Credit.Domain.DomainEvents
{
    public class CounterOfferMadeEvent : DomainEventBase
    {
        public CounterOfferMadeEvent(CreditDecision decision, string offeredBy)
        {
            EntityId = decision.Id;
            EntityType = nameof(CreditDecision);
            DecisionId = decision.DecisionId;
            ApplicationId = decision.ApplicationId;
            CustomerId = decision.CustomerId;
            OfferedBy = offeredBy;
            CounterOfferAmount = decision.CounterOfferAmount;
            CounterOfferInterestRate = decision.CounterOfferInterestRate;
            CounterOfferTermMonths = decision.CounterOfferTermMonths;
            CounterOfferExpiryDate = decision.CounterOfferExpiryDate;
            OfferDate = DateTime.UtcNow;
            OriginalApprovedAmount = decision.ApprovedAmount;
        }

        public string DecisionId { get; }
        public string ApplicationId { get; }
        public string CustomerId { get; }
        public string OfferedBy { get; }
        public decimal? CounterOfferAmount { get; }
        public decimal? CounterOfferInterestRate { get; }
        public int? CounterOfferTermMonths { get; }
        public DateTime? CounterOfferExpiryDate { get; }
        public DateTime OfferDate { get; }
        public decimal? OriginalApprovedAmount { get; }
    }
}
