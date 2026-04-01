namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class BulkStatusUpdateDto
    {
        public List<string> AccountIds { get; set; } = new();
        public string NewStatus { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string UpdateReason { get; set; } = string.Empty;
        public string? ApprovalReference { get; set; }
        public string? Comments { get; set; }
    }
}
