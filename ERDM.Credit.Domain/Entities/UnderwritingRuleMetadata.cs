
namespace ERDM.Credit.Domain.Entities
{
    public class UnderwritingRuleMetadata
    {
        public string? Source { get; set; }
        public string? Owner { get; set; }
        public string? Department { get; set; }
        public string? DeactivationReason { get; set; }
        public string? RejectionReason { get; set; }
        public string? LastFailureReason { get; set; }
        public DateTime? LastFailureAt { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}
