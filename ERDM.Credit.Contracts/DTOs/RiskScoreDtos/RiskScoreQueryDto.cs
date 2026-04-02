using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class RiskScoreQueryDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? CustomerId { get; set; }
        public string? ApplicationId { get; set; }
        public string? AccountId { get; set; }
        public string? ScoreType { get; set; }
        public string? RiskCategory { get; set; }
        public int? MinScore { get; set; }
        public int? MaxScore { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsValid { get; set; }
        public string SortBy { get; set; } = "ScoringDate";
        public bool SortDescending { get; set; } = true;
    }
}