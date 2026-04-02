using System;
using System.Collections.Generic;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class CustomerRiskProfileDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public RiskScoreResponseDto? CurrentRiskScore { get; set; }
        public List<RiskScoreResponseDto> ScoreHistory { get; set; } = new();
        public RiskCategory OverallRiskCategory { get; set; }
        public decimal ProbabilityOfDefault { get; set; }
        public decimal ExpectedLoss { get; set; }
        public List<RiskFactorDto> KeyRiskFactors { get; set; } = new();
        public DateTime LastAssessmentDate { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string? Recommendations { get; set; }
    }

}