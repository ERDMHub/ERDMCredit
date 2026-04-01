namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class BulkOperationResponseDto
    {
        public int TotalProcessed { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public List<BulkOperationErrorDto> Errors { get; set; } = new();
        public List<string> SuccessfulIds { get; set; } = new();
        public List<string> FailedIds { get; set; } = new();
        public DateTime ProcessedAt { get; set; }
        public string? ProcessedBy { get; set; }
    }
}
