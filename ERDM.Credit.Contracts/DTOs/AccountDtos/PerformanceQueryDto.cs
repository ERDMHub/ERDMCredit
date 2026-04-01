namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class PerformanceQueryDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? BranchCode { get; set; }
        public string? ProductType { get; set; }
        public string? AccountType { get; set; }
    }
}
