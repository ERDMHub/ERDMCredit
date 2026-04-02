using System;
using System.Collections.Generic;
using ERDM.Credit.Domain.Enums;

namespace ERDM.Credit.Contracts.DTOs.RiskScoreDtos
{
    public class RiskThresholdAlertDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string? AccountId { get; set; }
        public int CurrentScore { get; set; }
        public int ThresholdValue { get; set; }
        public string ThresholdType { get; set; } = string.Empty;
        public RiskCategory RiskCategory { get; set; }
        public DateTime AlertDate { get; set; }
        public string? RecommendedAction { get; set; }
    }
}