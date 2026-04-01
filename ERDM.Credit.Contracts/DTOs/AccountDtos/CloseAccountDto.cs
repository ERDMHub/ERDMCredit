namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class CloseAccountDto
    {
        public string ClosedBy { get; set; } = string.Empty;
        public string ClosureReason { get; set; } = string.Empty;
        public DateTime ClosureDate { get; set; } = DateTime.UtcNow;
        public decimal? FinalBalance { get; set; }
        public string? Comments { get; set; }
    }
}
