using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountWrittenOffEvent : DomainEventBase
    {
        public AccountWrittenOffEvent(Account account, string writtenOffBy, string writeOffReason, decimal writeOffAmount)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            WrittenOffBy = writtenOffBy;
            WriteOffReason = writeOffReason;
            WriteOffAmount = writeOffAmount;
            TotalPrincipalPaid = account.PrincipalAmount - account.OutstandingBalance;
            TotalInterestPaid = account.PaymentHistory.Sum(p => p.InterestPaid);
            DaysOverdueAtWriteOff = account.DaysOverdue;
            WriteOffDate = DateTime.UtcNow;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public string WrittenOffBy { get; }
        public string WriteOffReason { get; }
        public decimal WriteOffAmount { get; }
        public decimal TotalPrincipalPaid { get; }
        public decimal TotalInterestPaid { get; }
        public int DaysOverdueAtWriteOff { get; }
        public DateTime WriteOffDate { get; }
    }
}
