using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a limit is decreased
    public class LimitDecreasedEvent : DomainEventBase
    {
        public LimitDecreasedEvent(LimitHistory limitHistory)
        {
            EntityId = limitHistory.Id;
            EntityType = nameof(LimitHistory);
            LimitHistoryId = limitHistory.LimitHistoryId;
            CustomerId = limitHistory.CustomerId;
            AccountId = limitHistory.AccountId;
            AccountNumber = limitHistory.AccountNumber;
            PreviousLimit = limitHistory.PreviousLimit;
            NewLimit = limitHistory.NewLimit;
            DecreaseAmount = Math.Abs(limitHistory.ChangeAmount);
            DecreasePercentage = Math.Abs(limitHistory.ChangePercentage);
            Reason = limitHistory.Reason;
            ReasonCode = limitHistory.ReasonCode;
            ChangedBy = limitHistory.ChangedBy;
            EffectiveDate = limitHistory.EffectiveDate;
        }

        public string LimitHistoryId { get; }
        public string CustomerId { get; }
        public string AccountId { get; }
        public string AccountNumber { get; }
        public decimal PreviousLimit { get; }
        public decimal NewLimit { get; }
        public decimal DecreaseAmount { get; }
        public decimal DecreasePercentage { get; }
        public string Reason { get; }
        public string ReasonCode { get; }
        public string ChangedBy { get; }
        public DateTime EffectiveDate { get; }
    }
}