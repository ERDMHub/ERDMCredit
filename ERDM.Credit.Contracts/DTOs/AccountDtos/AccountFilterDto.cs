using ERDM.Credit.Domain.Entities;

namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountFilterDto
    {
        public string? CustomerId { get; set; }
        public AccountStatus? Status { get; set; }
        public AccountType? AccountType { get; set; }
        public string? ProductType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal? MinBalance { get; set; }
        public decimal? MaxBalance { get; set; }
        public string? BranchCode { get; set; }
        public string? AssignedOfficer { get; set; }
        public bool? IsDelinquent { get; set; }
        public int? MinOverdueDays { get; set; }
        public string? SearchTerm { get; set; }
    }
}
