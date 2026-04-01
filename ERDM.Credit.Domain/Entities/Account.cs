using ERDM.Core.Entities;

namespace ERDM.Credit.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string AccountId { get; set; } = Guid.NewGuid().ToString();
        public string AccountNumber { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public AccountStatus Status { get; set; }
        public AccountType AccountType { get; set; }

        // Financial Details
        public decimal PrincipalAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal OutstandingBalance { get; set; }
        public decimal AvailableCredit { get; set; }
        public string Currency { get; set; } = "USD";
        public decimal InterestRate { get; set; }
        public InterestType InterestType { get; set; }

        // Term Details
        public int TermMonths { get; set; }
        public int TermYears { get; set; }
        public RepaymentFrequency RepaymentFrequency { get; set; }
        public RepaymentMethod RepaymentMethod { get; set; }

        // Payment Details
        public decimal EmiAmount { get; set; }
        public int TotalPayments { get; set; }
        public int PaymentsMade { get; set; }
        public int PaymentsRemaining { get; set; }
        public DateTime? NextPaymentDueDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public decimal? LastPaymentAmount { get; set; }

        // Dates
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? MaturityDate { get; set; }

        // Account Management
        public string? AssignedOfficer { get; set; }
        public string? BranchCode { get; set; }
        public ChannelType Channel { get; set; }

        // Fees
        public decimal ProcessingFee { get; set; }
        public bool ProcessingFeePaid { get; set; }
        public decimal LatePaymentFee { get; set; }
        public PrepaymentPenalty? PrepaymentPenalty { get; set; }

        // Collections
        public bool IsDelinquent { get; set; }
        public int DaysOverdue { get; set; }
        public DateTime? LastCollectionAttempt { get; set; }
        public string? CollectionOfficer { get; set; }

        // Collateral
        public bool CollateralRequired { get; set; }
        public CollateralDetails? CollateralDetails { get; set; }

        // History and Metadata
        public List<AccountStatusHistory> StatusHistory { get; set; } = new();
        public List<PaymentHistory> PaymentHistory { get; set; } = new();
        public AccountMetadata Metadata { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public string? Notes { get; set; }
    }
}
