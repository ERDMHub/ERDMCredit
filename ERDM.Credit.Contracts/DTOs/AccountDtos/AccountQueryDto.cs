namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? CustomerId { get; set; }
        public string? AccountStatus { get; set; }
        public string? AccountType { get; set; }
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
        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }
}
