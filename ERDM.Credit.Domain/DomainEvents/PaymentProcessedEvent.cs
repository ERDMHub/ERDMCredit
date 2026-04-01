using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class PaymentProcessedEvent : DomainEventBase
    {
        public PaymentProcessedEvent(Account account, PaymentHistory payment)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            PaymentId = payment.PaymentId;
            PaymentAmount = payment.AmountPaid;
            PrincipalPaid = payment.PrincipalPaid;
            InterestPaid = payment.InterestPaid;
            FeesPaid = payment.FeesPaid;
            PaymentDate = payment.PaymentDate;
            PaymentMethod = payment.PaymentMethod;
            TransactionReference = payment.TransactionReference;
            RemainingBalance = account.OutstandingBalance;
            IsFullPayment = account.OutstandingBalance == 0;
            IsLatePayment = payment.LateDays > 0;
            LateDays = payment.LateDays;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public string PaymentId { get; }
        public decimal PaymentAmount { get; }
        public decimal PrincipalPaid { get; }
        public decimal InterestPaid { get; }
        public decimal FeesPaid { get; }
        public DateTime PaymentDate { get; }
        public RepaymentMethod PaymentMethod { get; }
        public string TransactionReference { get; }
        public decimal RemainingBalance { get; }
        public bool IsFullPayment { get; }
        public bool IsLatePayment { get; }
        public int LateDays { get; }
    }
}
