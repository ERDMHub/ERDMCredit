using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Entities
{
    public class UnderwritingRuleFilter
    {
        public string? RuleName { get; set; }
        public string? RuleCode { get; set; }
        public RuleType? RuleType { get; set; }
        public RuleCategory? Category { get; set; }
        public RuleStatus? Status { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
    }
}
