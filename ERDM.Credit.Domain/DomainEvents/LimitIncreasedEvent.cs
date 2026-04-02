using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    // Event raised when a limit is increased
    public class LimitIncreasedEvent : DomainEventBase
    {
        public LimitIncreasedEvent(LimitHistory limitHistory)
        {
            EntityId = limitHistory.Id;
            EntityType = nameof(LimitHistory);
            LimitHistoryId = limitHistory.LimitHistoryId;
            CustomerId = limitHistory.CustomerId;
            AccountId = limitHistory.AccountId;
            AccountNumber = limitHistory.AccountNumber;
            PreviousLimit = limitHistory.PreviousLimit;
            NewLimit = limitHistory.NewLimit;
            IncreaseAmount = limitHistory.ChangeAmount;
            IncreasePercentage = limitHistory.ChangePercentage;
            Reason = limitHistory.Reason;
            ReasonCode = limitHistory.ReasonCode;
            ChangedBy = limitHistory.ChangedBy;
            EffectiveDate = limitHistory.EffectiveDate;
            IsTemporary = limitHistory.IsTemporary;
            TemporaryDurationDays = limitHistory.TemporaryDurationDays;
        }

        public string LimitHistoryId { get; }
        public string CustomerId { get; }
        public string AccountId { get; }
        public string AccountNumber { get; }
        public decimal PreviousLimit { get; }
        public decimal NewLimit { get; }
        public decimal IncreaseAmount { get; }
        public decimal IncreasePercentage { get; }
        public string Reason { get; }
        public string ReasonCode { get; }
        public string ChangedBy { get; }
        public DateTime EffectiveDate { get; }
        public bool IsTemporary { get; }
        public int? TemporaryDurationDays { get; }
    }

}