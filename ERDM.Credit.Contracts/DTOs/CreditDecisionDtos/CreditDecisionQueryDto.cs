namespace ERDM.Credit.Contracts.DTOs.CreditDecisionDtos
{
    public class CreditDecisionQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? ApplicationId { get; set; }
        public string? CustomerId { get; set; }
        public string? DecisionType { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? DecisionBy { get; set; }
        public string? RiskGrade { get; set; }
        public string SortBy { get; set; } = "DecisionDate";
        public bool SortDescending { get; set; } = true;
    }
}
