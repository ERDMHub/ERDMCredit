namespace ERDM.Credit.Contracts.DTOs.AccountDtos
{
    public class AccountMetadataDto
    {
        public string Source { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceId { get; set; }
        public string? ApprovalReference { get; set; }
        public string? DisbursementReference { get; set; }
        public bool ContractSigned { get; set; }
        public DateTime? ContractSignedDate { get; set; }
        public List<string> DocumentsUploaded { get; set; } = new();
        public bool AutoDebitEnabled { get; set; }
        public string? AutoDebitAccount { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool EmailAlerts { get; set; }
        public bool SmsAlerts { get; set; }
    }

}
