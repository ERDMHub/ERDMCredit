namespace ERDM.Credit.Domain.Entities
{
    public class AccountStatusHistory
    {
        public AccountStatus Status { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? Comments { get; set; }
    }
}
