namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class CreateAccountDto
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal ApprovedAmount { get; set; }
        public decimal PrincipalAmount { get; set; }
        public int TermMonths { get; set; }
        public decimal InterestRate { get; set; }
        public string InterestType { get; set; } = string.Empty;
        public string RepaymentFrequency { get; set; } = string.Empty;
        public string RepaymentMethod { get; set; } = string.Empty;
        public string Currency { get; set; } = "USD";
        public string? BranchCode { get; set; }
        public string? AssignedOfficer { get; set; }
        public CollateralDetailsDto? CollateralDetails { get; set; }
        public string? Notes { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
