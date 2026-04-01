namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountStatisticsQueryDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? CustomerId { get; set; }
        public string? BranchCode { get; set; }
        public string? ProductType { get; set; }
        public bool IncludeDetails { get; set; } = false;
    }
}
