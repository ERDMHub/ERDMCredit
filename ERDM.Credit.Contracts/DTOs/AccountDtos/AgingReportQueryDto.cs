namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AgingReportQueryDto
    {
        public DateTime AsOfDate { get; set; } = DateTime.UtcNow;
        public string? BranchCode { get; set; }
        public string? ProductType { get; set; }
        public string? AssignedOfficer { get; set; }
        public bool IncludeZeroBalance { get; set; } = false;
    }
}
