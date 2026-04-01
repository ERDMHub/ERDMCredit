namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class ActivateAccountDto
    {
        public string ActivatedBy { get; set; } = string.Empty;
        public string ActivationReason { get; set; } = string.Empty;
        public DateTime ActivationDate { get; set; } = DateTime.UtcNow;
        public string? Comments { get; set; }
    }
}
