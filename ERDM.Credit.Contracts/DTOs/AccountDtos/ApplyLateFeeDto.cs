namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class ApplyLateFeeDto
    {
        public decimal LateFeeAmount { get; set; }
        public string AppliedBy { get; set; } = string.Empty;
        public int DaysOverdue { get; set; }
        public DateTime FeeAppliedDate { get; set; } = DateTime.UtcNow;
        public bool AddToBalance { get; set; } = true;
        public string? Comments { get; set; }
    }
}
