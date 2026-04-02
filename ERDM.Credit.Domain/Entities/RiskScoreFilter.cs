using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Domain.Entities
{
    public class RiskScoreFilter
    {
        public string? CustomerId { get; set; }
        public string? ApplicationId { get; set; }
        public string? AccountId { get; set; }
        public ScoreType? ScoreType { get; set; }
        public RiskCategory? RiskCategory { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsValid { get; set; }
        public bool? RequiresReview { get; set; }
    }
}