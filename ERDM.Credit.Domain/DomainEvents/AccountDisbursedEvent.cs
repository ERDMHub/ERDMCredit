using ERDM.Core.Entities;
using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Domain.DomainEvents
{
    public class AccountDisbursedEvent : DomainEventBase
    {
        public AccountDisbursedEvent(Account account, DisbursementDetails disbursementDetails)
        {
            EntityId = account.Id;
            EntityType = nameof(Account);
            AccountId = account.AccountId;
            AccountNumber = account.AccountNumber;
            CustomerId = account.CustomerId;
            ApplicationId = account.ApplicationId;
            DisbursedAmount = disbursementDetails.Amount;
            DisbursementDate = disbursementDetails.DisbursementDate;
            DisbursementMethod = disbursementDetails.DisbursementMethod;
            BankAccountNumber = disbursementDetails.BankAccountNumber;
            BankName = disbursementDetails.BankName;
            TransactionReference = disbursementDetails.TransactionReference;
            DisbursedBy = disbursementDetails.DisbursedBy;
            FirstPaymentDueDate = account.NextPaymentDueDate;
        }

        public string AccountId { get; }
        public string AccountNumber { get; }
        public string CustomerId { get; }
        public string ApplicationId { get; }
        public decimal DisbursedAmount { get; }
        public DateTime DisbursementDate { get; }
        public string DisbursementMethod { get; }
        public string? BankAccountNumber { get; }
        public string? BankName { get; }
        public string TransactionReference { get; }
        public string DisbursedBy { get; }
        public DateTime? FirstPaymentDueDate { get; }
    }
}
