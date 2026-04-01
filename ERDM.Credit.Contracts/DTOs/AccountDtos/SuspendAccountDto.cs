namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class SuspendAccountDto
    {
        public string SuspendedBy { get; set; } = string.Empty;
        public string SuspensionReason { get; set; } = string.Empty;
        public DateTime SuspensionDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedResumptionDate { get; set; }
        public string? Comments { get; set; }
    }
}
