namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class BulkOverdueDto
    {
        public DateTime AsOfDate { get; set; } = DateTime.UtcNow;
        public int MinDaysOverdue { get; set; } = 1;
        public int MaxDaysOverdue { get; set; } = int.MaxValue;
        public string? BranchCode { get; set; }
        public string? ProductType { get; set; }
        public string MarkedBy { get; set; } = string.Empty;
        public bool AutoAssignCollections { get; set; } = true;
    }

}
