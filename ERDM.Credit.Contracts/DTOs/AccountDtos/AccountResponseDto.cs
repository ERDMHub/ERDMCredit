namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string ApplicationId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string AccountStatus { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;

        // Financial Details
        public decimal PrincipalAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal OutstandingBalance { get; set; }
        public decimal AvailableCredit { get; set; }
        public string Currency { get; set; } = "USD";
        public decimal InterestRate { get; set; }
        public string InterestType { get; set; } = string.Empty;

        // Term Details
        public int TermMonths { get; set; }
        public int TermYears { get; set; }
        public string RepaymentFrequency { get; set; } = string.Empty;
        public string RepaymentMethod { get; set; } = string.Empty;

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
        public string Channel { get; set; } = string.Empty;

        // Fees
        public decimal ProcessingFee { get; set; }
        public bool ProcessingFeePaid { get; set; }
        public decimal LatePaymentFee { get; set; }

        // Collections
        public bool IsDelinquent { get; set; }
        public int DaysOverdue { get; set; }
        public DateTime? LastCollectionAttempt { get; set; }
        public string? CollectionOfficer { get; set; }

        // Collateral
        public bool CollateralRequired { get; set; }
        public CollateralDetailsDto? CollateralDetails { get; set; }

        // Metadata
        public AccountMetadataDto Metadata { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public string? Notes { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
    }
}
