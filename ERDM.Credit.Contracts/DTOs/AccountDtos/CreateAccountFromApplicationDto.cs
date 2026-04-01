namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class CreateAccountFromApplicationDto
    {
        public decimal ApprovedAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermMonths { get; set; }
        public string RepaymentMethod { get; set; } = string.Empty;
        public string? BranchCode { get; set; }
        public string? AssignedOfficer { get; set; }
        public string? Notes { get; set; }
    }
}
