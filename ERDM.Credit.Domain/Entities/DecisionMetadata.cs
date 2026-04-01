namespace ERDM.Credit.Domain.Entities
{
    public class DecisionMetadata
    {
        public string Source { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceId { get; set; }
        public string? WorkflowId { get; set; }
        public DateTime? AutoApprovalDate { get; set; }
        public bool IsAutoApproved { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}
