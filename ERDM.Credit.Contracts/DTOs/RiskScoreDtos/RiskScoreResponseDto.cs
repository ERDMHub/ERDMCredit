using System;
using System.Collections.Generic;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class RiskScoreResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string RiskScoreId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string? ApplicationId { get; set; }
        public string? AccountId { get; set; }
        public string ScoreType { get; set; } = string.Empty;
        public int ScoreValue { get; set; }
        public int? PreviousScore { get; set; }
        public int ScoreChange { get; set; }
        public string ScoreGrade { get; set; } = string.Empty;
        public string RiskCategory { get; set; } = string.Empty;
        public decimal ProbabilityOfDefault { get; set; }
        public decimal LossGivenDefault { get; set; }
        public decimal ExposureAtDefault { get; set; }
        public decimal ExpectedLoss { get; set; }
        public List<RiskFactorDto> RiskFactors { get; set; } = new();
        public DateTime ScoringDate { get; set; }
        public string ScoredBy { get; set; } = string.Empty;
        public string ScoringModel { get; set; } = string.Empty;
        public string ScoringModelVersion { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime? NextReviewDate { get; set; }
        public string? ReviewNotes { get; set; }
        public RiskScoreMetadataDto Metadata { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

}