using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class CreateRiskScoreDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string? ApplicationId { get; set; }
        public string? AccountId { get; set; }
        public string ScoreType { get; set; } = string.Empty;
        public int ScoreValue { get; set; }
        public string ScoreGrade { get; set; } = string.Empty;
        public string RiskCategory { get; set; } = string.Empty;
        public string ScoredBy { get; set; } = string.Empty;
        public string ScoringModel { get; set; } = string.Empty;
        public string ScoringModelVersion { get; set; } = string.Empty;
        public List<CreateRiskFactorDto>? RiskFactors { get; set; }
        public decimal? ProbabilityOfDefault { get; set; }
        public decimal? LossGivenDefault { get; set; }
        public decimal? ExposureAtDefault { get; set; }
        public List<string>? Tags { get; set; }
    }

}