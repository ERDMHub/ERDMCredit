namespace ERDM.Credit.Domain.Entities
{
      public class RiskScoreMetadata
    {
        public string? Source { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? WorkflowId { get; set; }
        public Dictionary<string, object>? AdditionalData { get; set; }
    }
}